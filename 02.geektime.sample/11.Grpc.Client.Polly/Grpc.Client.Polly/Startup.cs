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
             ʧ������
             */
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true); //����ʹ�ò����ܵ�HTTP/2Э��
            services.AddGrpcClient<OrderGrpc.OrderGrpcClient>(options =>
            {
                options.Address = new Uri("https://localhost://5001");
            }).ConfigurePrimaryHttpMessageHandler(options =>
            {
                var handler = new SocketsHttpHandler();
                handler.SslOptions.RemoteCertificateValidationCallback = (a, b, c, d) => true; //������Ч����ǩ��֤��
                return handler;
            })
            .AddTransientHttpErrorPolicy(s => s.WaitAndRetryForeverAsync(s => TimeSpan.FromSeconds(s * 1)));


            /*
             ʧ������
             */
            // �Զ�Polly�����
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
             �۶�����
             */

            services.AddHttpClient("orderClientV3")
                // �������
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                //�����۶ϲ���
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking:10,// ������ٴν����۶�
                    durationOfBreak:TimeSpan.FromSeconds(10),// �۶�ʱ��
                    onBreak: (r,t) => 
                    { // �����۶�ʱ������һ���¼�
                    
                    },
                    onReset: () => 
                    { // �۶ϻָ�ʱ������һ���¼�
                    
                    },
                    onHalfOpen: () => 
                    {// �ָ�֮ǰ��֤�����Ƿ����
                    
                    }

                    ));


            //�߼��۶ϲ��ԣ����Ǹ�������ʧ�ܴ��������Ǹ���ʧ�ܵı���
            services.AddHttpClient("orderClientV4")
                // �������
                .AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                // �����۶ϲ���
                .AdvancedCircuitBreakerAsync(
                    failureThreshold: 0.6, //����ʧ�ܱ���
                    samplingDuration: TimeSpan.FromSeconds(30),// ���ٷ�Χ������Ҫ60%��ʧ�ܱ������۶�:��30s��ʧ�ܱ�����60%���۶�
                    minimumThroughput: 100,// ������������100���Żᴥ���۶ϻ��ƣ�������С�ǲ���Ҫ�۶ϵģ�������������ֻ��5����ʧ����3���ͻᴥ���������õķ���ʧ�ܱ����ﵽ60%ʱ�����۶ϣ��˴����õ���С������Ϊ100��ʱ�Żᴥ���۶�
                    durationOfBreak: TimeSpan.FromSeconds(20),// �۶�ʱ��
                    onBreak: (r, t) =>
                    {// �۶�ʱ�������¼�

                    },
                    onReset: () =>
                    {// �۶ϻָ�ʱ�������¼�

                    },
                    onHalfOpen: () =>
                    {// �۶ϻظ�ʱ��֤�����Ƿ����

                    }

                    ));


            // �����۶ϲ���
            #region �۶�_��ϲ���
            var broken_Policy = Policy<HttpResponseMessage>.Handle<HttpRequestException>()
                   // �����۶ϲ���
                   .AdvancedCircuitBreakerAsync(
                       failureThreshold: 0.6, //����ʧ�ܱ���
                       samplingDuration: TimeSpan.FromSeconds(30),// ���ٷ�Χ������Ҫ60%��ʧ�ܱ������۶�:��30s��ʧ�ܱ�����60%���۶�
                       minimumThroughput: 100,// ������������100���Żᴥ���۶ϻ��ƣ�������С�ǲ���Ҫ�۶ϵģ�������������ֻ��5����ʧ����3���ͻᴥ���������õķ���ʧ�ܱ����ﵽ60%ʱ�����۶ϣ��˴����õ���С������Ϊ100��ʱ�Żᴥ���۶�
                       durationOfBreak: TimeSpan.FromSeconds(20),// �۶�ʱ��
                       onBreak: (r, t) =>
                       {// �۶�ʱ�������¼�

                       },
                       onReset: () =>
                       {// �۶ϻָ�ʱ�������¼�

                       },
                       onHalfOpen: () =>
                       {// �۶ϻظ�ʱ��֤�����Ƿ����

                       }

                       );

            // �����۶ϲ��Է��ص��Զ����к���Ϣ
            var broken_Message = new HttpResponseMessage { Content = new StringContent("{ }") };
            //�۶ϵĻص�
            var broken_fallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync(broken_Message);
            // �������Բ���
            var broken_retryPolicy = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, s => TimeSpan.FromSeconds(1));
            var broken_wrap = Policy.WrapAsync(broken_fallback, broken_retryPolicy, broken_Policy);
            services.AddHttpClient("httpV1").AddPolicyHandler(broken_wrap);
            #endregion

            if ("false" == bool.FalseString)
            {
            }
            /*
             ����
             */
            #region ����_��ϲ���
            var bulk_Policy = Policy.BulkheadAsync<HttpResponseMessage>(
                        maxQueuingActions: 20,//��������20
                        maxParallelization: 30,//���������30
                        onBulkheadRejectedAsync: context => Task.CompletedTask //������Ĵ���
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
