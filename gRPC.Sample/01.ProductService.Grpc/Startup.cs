using _01.ProductService.Grpc.Service;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ProtoBuf.Grpc.Server;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _01.ProductService.Grpc
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
            // �ؼ�����
            services.AddCodeFirstGrpc();//ע�������˴������ȵķ���
            // �ؼ�����



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
                // �ؼ����� 
                endpoints.MapGrpcService<OrderService>();//��Ӵ������ȵķ����ս�㡣
                // �ؼ�����

                endpoints.MapControllers();
            });
        }
    }
}
