using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _05.Configuration.OptionDemo.Service;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _05.Configuration.OptionDemo
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
            //services.Configure<UserInfoOptions>(Configuration.GetSection(nameof(UserInfoOptions)));
            //services.AddScoped<IUserInfoService, UserInfoService>();
            //services.AddSingleton<IUserInfoService, UserInfoService>();// ʹ�õ���ģʽע����� IOptionsSnapshot �޷���ȡ����ʱ���Ը�ΪIOptionsMonitor

            /* ע�����̫��Ļ�����д����չ����   ʹConfigurationService �еĴ�����Ӽ��*/
            services.AddUserInfoService(Configuration);

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
                endpoints.MapControllers();
            });
        }
    }
}
