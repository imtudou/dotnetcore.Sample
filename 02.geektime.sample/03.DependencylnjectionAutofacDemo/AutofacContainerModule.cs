using _03.DependencylnjectionAutofacDemo.Service;

using Autofac;
using Autofac.Extras.DynamicProxy;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _03.DependencylnjectionAutofacDemo
{
    public class AutofacContainerModule : Autofac.Module
    {
        private static IContainer _container;
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // 1.注册拦截器
            builder.RegisterType<MyInterceptor>();

            //2.
            var controllerBaseType = typeof(ControllerBase);
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
                .PropertiesAutowired() // 允许属性注入
                .EnableClassInterceptors(); // 允许在Controller类上使用拦截器

            //常规注册
            builder.RegisterType<LogUtil>().As<ILogUtil>().EnableInterfaceInterceptors(); // 允许在Interface上使用拦截器
            
            /*
            第一种：给类型加上特性Attribute
                [Intercept(typeof(MyInterceptor))]

            第二种：在注册类型到容器的时候动态注入拦截器
                builder.RegisterType<MyNameService2>().InterceptedBy(typeof(MyInterceptor)).EnableClassInterceptors();
            */

            builder.RegisterType<MyNameService2>().EnableClassInterceptors();// 允许在Class上使用拦截器


            //一个接口多个实现 为不同的实现指定名称
            builder.RegisterType<MyService2>().Named<IMyService>("service2").EnableInterfaceInterceptors();// 允许在Interface上使用拦截器

            //属性注册
            builder.RegisterType<MyNameService>().PropertiesAutowired();

            //
            builder.RegisterBuildCallback(s => _container = (IContainer)s);

        }

        /// <summary>
        /// 获取容器 IContainer 方法
        /// </summary>
        /// <returns></returns>
        public static IContainer GetContainer()
        {
            return _container;
        }
    }
}
