using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _02.DependencylnjectionDemo.Service;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace _02.DependencylnjectionDemo
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
            #region ע�᲻ͬ�������������

            services.AddSingleton<IMySingletonService, MySingletonService>();
            services.AddScoped<IMyScopedService, MyScopedService>();
            //services.AddTransient<IMyTransientService, MyTransientService>();
            #endregion


            #region ��ʽע��
            //// ֱ��ע��ʵ��
            //services.AddSingleton<IMySingletonService>(new MySingletonService());

            //// ������ʽע��
            //services.AddSingleton<IMyScopedService>(serviceProvider =>
            //{
            //    return new MyScopedService();
            //});

            //services.AddTransient<IMyTransientService>(serviceProvider =>
            //{
            //    //serviceProvider.GetService<>
            //    return new MyTransientService();
            //});


            #endregion

            #region ����ע��
            //services.TryAddEnumerable(ServiceDescriptor.Singleton<IMySingletonService, MySingletonService>());
            //services.TryAddSingleton<IMySingletonService, MySingletonService>();
            //services.TryAddScoped<IMyScopedService, MyScopedService>();
            //services.TryAddTransient<IMyScopedService, MyScopedService>(); 
            #endregion


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // �Ӹ���������ȡ˲ʱ����
            var s1 = app.ApplicationServices.GetService<IMySingletonService>();
            var ss1 = s1.GetService();

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
