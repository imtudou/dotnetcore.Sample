using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GrpcService.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace GrpcService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableDetailedErrors = true;//是否输出日志详细信息（生产环境不建议）
                //options.Interceptors.Add<>();// 配置拦截器
            });
            services.AddTransient<OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            var service = app.ApplicationServices.GetService<OrderService>();
            var result = service.CreateOrder(new CreateOrderInput { BuyerName = "张小三" }, null);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<OrderService>();

                endpoints.MapGet("/", async context =>
                {                  
                    context.Response.ContentType = "text/plain;charset=utf-8";
                    await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
                });
            });
        }
    }
}
