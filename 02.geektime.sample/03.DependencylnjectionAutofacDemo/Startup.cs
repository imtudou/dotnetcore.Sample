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
     参考：https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
     
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

        //在注册模块注册拦截器并启用AOP拦截
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacContainerModule());
        }

        /*
         获取容器 IContainer
                定义一个继承自Autofac.Module的类，重写其Load方法，将Startup.cs中ConfigurationContainer方法中的代码全部移至Load的方法内，
         */
        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    //builder.RegisterType<MyService>().As<IMyService>();

        //    #region 命名注册
        //    //builder.RegisterType<MyService2>().Named<IMyService>("service2");
        //    #endregion

        //    #region 属性注册
        //    //如果需要在Controller中使用属性注入，需要在ConfigureContainer中添加如下代码
        //    //var controllerBaseType = typeof(ControllerBase);
        //    //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //    //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        //    //    .PropertiesAutowired();

        //    //builder.RegisterType<MyService>().PropertiesAutowired();
        //    #endregion

        //    #region AOP
        //    builder.RegisterType<MyInterceptor>();// 要先注册拦截器


        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //        .AsImplementedInterfaces()
        //        .EnableInterfaceInterceptors();// 允许在Interface上使用拦截器

        //    var controllerBaseType = typeof(ControllerBase);
        //    builder.RegisterAssemblyTypes(typeof(Program).Assembly)
        //        .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
        //        .PropertiesAutowired() // 允许属性注入
        //        .EnableClassInterceptors(); // 允许在Controller类上使用拦截器


        //    /*
        //        1、拦截器注册要在使用拦截器的接口和类型之前
        //        2、在类型中使用，仅virtual方法可以触发拦截器
        //     */


        //    //一个接口多个实现

        //    //1.为不同的实现指定名称
        //    builder.RegisterType<MyService2>().Named<IMyService>("service2");
        //    /*
        //        // 根据名称解析
        //        var t2 = container.ResolveNamed<ITestServiceD>("three");
        //    */
        //    builder.RegisterType<MyService>().PropertiesAutowired();

        //    // 设置该类型允许AOP拦截
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
