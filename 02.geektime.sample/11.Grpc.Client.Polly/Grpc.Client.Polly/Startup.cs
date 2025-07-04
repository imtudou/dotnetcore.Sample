using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using GrpcService;
using Polly;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using Polly.Bulkhead;

namespace Grpc.Client.Polly
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            /*
             失败重试
             */
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true); //允许使用不加密的HTTP/2协议
            services.AddGrpcClient<OrderGrpc.OrderGrpcClient>(options =>
            {
                options.Address = new Uri("https://localhost://5001");
            }).ConfigurePrimaryHttpMessageHandler(options =>
            {
                var handler = new SocketsHttpHandler();
                handler.SslOptions.RemoteCertificateValidationCallback = (a, b, c, d) => true; //允许无效，自签名证书
                return handler;
            })
            .AddTransientHttpErrorPolicy(s => s.WaitAndRetryForeverAsync(s => TimeSpan.FromSeconds(s * 1)));


            /*
             失败重试
             */
            // 自定Polly义策略
            var reg = services.AddPolicyRegistry();

            reg.Add("retryForver", Policy.HandleResult<HttpResponseMessage>(message =>
            {
                return message.StatusCode == System.Net.HttpStatusCode.Created;
            }).RetryForeverAsync());

            services.AddHttpClient("orderClientV1").AddPolicyHandlerFromRegistry("retryForver");

            services.AddHttpClient("orderClientV2").AddPolicyHandlerFromRegistry((registry, message) =>
            {
                return message.Method == HttpMethod.Get ? registry.Get<IAsyncPolicy<HttpResponseMessage>>("retryForver") : Policy.NoOpAsync<HttpResponseMessage>();
            });



            /*
             熔断限流
             */

            services.AddHttpClient("orderClientV3")
                // 定义策略
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                //定义熔断策略
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking:10,// 报错多少次进行熔断
                    durationOfBreak:TimeSpan.FromSeconds(10),// 熔断时间
                    onBreak: (r,t) => 
                    { // 发送熔断时触发的一个事件
                    
                    },
                    onReset: () => 
                    { // 熔断恢复时触发的一个事件
                    
                    },
                    onHalfOpen: () => 
                    {// 恢复之前验证服务是否可用
                    
                    }

                    ));


            //高级熔断策略：不是根据请求失败次数，而是根据失败的比例
            services.AddHttpClient("orderClientV4")
                // 定义策略
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                // 定义熔断策略
                .AdvancedCircuitBreakerAsync(
                    failureThreshold: 0.6, //服务失败比例
                    samplingDuration: TimeSpan.FromSeconds(30),// 多少范围内请求要60%的失败比例才熔断:即30s内失败比例有60%才熔断
                    minimumThroughput: 100,// 请求量最少有100个才会触发熔断机制，请求量小是不需要熔断的，比如我们请求只有5个，失败了3个就会触发上述配置的服务失败比例达到60%时触发熔断，此处配置的最小请求量为100个时才会触发熔断
                    durationOfBreak: TimeSpan.FromSeconds(20),// 熔断时长
                    onBreak: (r, t) =>
                    {// 熔断时触发的事件

                    },
                    onReset: () =>
                    {// 熔断恢复时触发的事件

                    },
                    onHalfOpen: () =>
                    {// 熔断回复时验证服务是否可用

                    }

                    ));


            // 定义熔断策略
            #region 熔断_组合策略
            var broken_Policy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                   // 定义熔断策略
                   .AdvancedCircuitBreakerAsync(
                       failureThreshold: 0.6, //服务失败比例
                       samplingDuration: TimeSpan.FromSeconds(30),// 多少范围内请求要60%的失败比例才熔断:即30s内失败比例有60%才熔断
                       minimumThroughput: 100,// 请求量最少有100个才会触发熔断机制，请求量小是不需要熔断的，比如我们请求只有5个，失败了3个就会触发上述配置的服务失败比例达到60%时触发熔断，此处配置的最小请求量为100个时才会触发熔断
                       durationOfBreak: TimeSpan.FromSeconds(20),// 熔断时长
                       onBreak: (r, t) =>
                       {// 熔断时触发的事件

                       },
                       onReset: () =>
                       {// 熔断恢复时触发的事件

                       },
                       onHalfOpen: () =>
                       {// 熔断回复时验证服务是否可用

                       }

                       );

            // 定义熔断策略返回的自定义有好消息
            var broken_Message = new HttpResponseMessage { Content = new StringContent("{ }") };
            //熔断的回调
            var broken_fallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync(broken_Message);
            // 定义重试策略
            var broken_retryPolicy = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, s => TimeSpan.FromSeconds(1));
            var broken_wrap = Policy.WrapAsync(broken_fallback, broken_retryPolicy, broken_Policy);
            services.AddHttpClient("httpV1").AddPolicyHandler(broken_wrap);
            #endregion

            if ("false" == bool.FalseString)
            {
            }
            /*
             限流
             */
            #region 限流_组合策略
            var bulk_Policy = Policy.BulkheadAsync<HttpResponseMessage>(
                        maxQueuingActions: 20,//最大队列数20
                        maxParallelization: 30,//并发最多是30
                        onBulkheadRejectedAsync: context => Task.CompletedTask //限流后的处理
                    );

            var bulk_Message = new HttpResponseMessage { Content = new StringContent("{ }") };
            var bulk_fallback = Policy<HttpResponseMessage>.Handle<BulkheadRejectedException>().FallbackAsync(bulk_Message);
            var bulk_warp = Policy.WrapAsync(bulk_fallback, bulk_Policy);
            services.AddHttpClient("httpV2").AddPolicyHandler(bulk_warp);

            #endregion






            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapGet("/",async context =>
                 {
                     var orderService = app.ApplicationServices.GetService<OrderGrpc.OrderGrpcClient>();
                     var result = orderService.CreateOrder(new CreateOrderInput
                     {
                         BuyerId = 1,
                         BuyerName = "",
                     });

                     context.Request.ContentType = "text/plain;charset=utf-8";
                     var data = System.Text.Json.JsonSerializer.Serialize(result);
                     await context.Response.WriteAsync(data);
                 });

            });
        }
    }
}
