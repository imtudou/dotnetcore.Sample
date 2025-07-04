using Autofac.Extras.DynamicProxy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _03.DependencylnjectionAutofacDemo.Service
{
    [Intercept(typeof(MyInterceptor))]
    public interface IMyService 
    {
        string ShowCode();
        string ShowMsg(string msg);
    }
    public class MyService : IMyService
    {
        public string ShowCode()
        {
            Console.WriteLine($"MyService.ShowCode：{GetHashCode()}");
            return string.Empty;
        }

        public string ShowMsg(string msg)
        {
            return $"MyService.ShowMsg：[{msg}]" + DateTime.Now;
        }
    }

    public class MyService2 : IMyService
    {
        public string ShowCode()
        {
            Console.WriteLine($"MyService2.ShowCode：{GetHashCode()}");
            return string.Empty;
        }

        public string ShowMsg(string msg)
        {
            return $"MyService2.ShowMsg：[{msg}]" + DateTime.Now;
        }
    }


    

    public class MyNameService
    {
        public string ShowMsg(string msg)
        {
            return $"MyNameService.ShowMsg：[{msg}]" + DateTime.Now;
        }

    }

    [Intercept(typeof(MyInterceptor))]
    public class MyNameService2
    {
        /*
            在注册对象的同时启用 EnableClassInterceptors() 方法。
     对于以类方式的注入，Autofac Interceptor 要求类的方法为必须为 virtual 方法
         */
        public virtual string ShowMsg(string msg)
        {
            return $"MyNameService2.ShowMsg：[{msg}]" + DateTime.Now;
        }
    }
}
