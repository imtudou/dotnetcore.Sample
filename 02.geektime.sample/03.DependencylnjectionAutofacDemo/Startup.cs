using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using _03.DependencylnjectionAutofacDemo.Service;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using _03.DependencylnjectionAutofacDemo.Controllers;

namespace _03.DependencylnjectionAutofacDemo
{

    /*
     �ο���https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
     
     */
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        public ILifetimeScope AutofacContainer { get; private set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();
        }

        //��ע��ģ��ע��������������AOP����
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacContainerModule());
        }

        /*
         ��ȡ���� IContainer
                ����һ���̳���Autofac.Module���࣬��д��Load��������Startup.cs��ConfigurationContainer�����еĴ���ȫ������Load�ķ����ڣ�
         */
        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    //builder.RegisterType<MyService>().As<IMyService>();

        //    #region ����ע��
        //    //builder.RegisterType<MyService2>().Named<IMyService>("service2");
        //    #endregion

        //    #region ����ע��
        //    //�����Ҫ��Controller��ʹ������ע�룬��Ҫ��ConfigureContainer��������´���
        //    //var controllerBaseType = typeof(ControllerBase);
        //    //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //    //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        //    //    .PropertiesAutowired();

        //    //builder.RegisterType<MyService>().PropertiesAutowired();
        //    #endregion

        //    #region AOP
        //    builder.RegisterType<MyInterceptor>();// Ҫ��ע��������


        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //        .AsImplementedInterfaces()
        //        .EnableInterfaceInterceptors();// ������Interface��ʹ��������

        //    var controllerBaseType = typeof(ControllerBase);
        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        //        .PropertiesAutowired() // ��������ע��
        //        .EnableClassInterceptors(); // ������Controller����ʹ��������


        //    /*
        //        1��������ע��Ҫ��ʹ���������Ľӿں�����֮ǰ
        //        2����������ʹ�ã���virtual�������Դ���������
        //     */


        //    //һ���ӿڶ��ʵ��

        //    //1.Ϊ��ͬ��ʵ��ָ������
        //    builder.RegisterType<MyService2>().Named<IMyService>("service2");
        //    /*
        //        // �������ƽ���
        //        var t2 = container.ResolveNamed<ITestServiceD>("three");
        //    */
        //    builder.RegisterType<MyService>().PropertiesAutowired();

        //    // ���ø���������AOP����
        //    builder.RegisterType<LogUtil>().As<ILogUtil>().EnableInterfaceInterceptors();



        //    #endregion






        //}

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
