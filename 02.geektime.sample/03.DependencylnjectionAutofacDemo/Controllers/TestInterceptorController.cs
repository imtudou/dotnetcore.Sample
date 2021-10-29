using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _03.DependencylnjectionAutofacDemo.Service;

using Autofac;
using Autofac.Extras.DynamicProxy;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _03.DependencylnjectionAutofacDemo.Controllers
{

    /*
     1、拦截器注册要在使用拦截器的接口和类型之前
     2、在类型中使用，仅virtual方法可以触发拦截器
     */

    [Intercept(typeof(MyInterceptor))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestInterceptorController : ControllerBase
    {
        private readonly ILogUtil _logUtil;     

        //属性获取
        public MyNameService NameService { get; set; }


        public TestInterceptorController(ILogUtil log)
        {
            _logUtil = log;
        }


        [HttpGet]
        public string GetService1()
        {
            // 一个接口多个实现 为不同的实现指定名称
            var container = AutofacContainerModule.GetContainer();
            var myService = container.ResolveNamed<IMyService>("service2");
            var result = myService.ShowMsg("一个接口多个实现 为不同的实现指定名称 会触发拦截器");
            return result;
        }

        [HttpGet]
        public string GetService2()
        {
            // 属性注册 不会触发拦截器
            var result = NameService.ShowMsg("属性注入");
            return result;
        }

        [HttpGet]
        public string GetService3()
        {
           return _logUtil.ShowMsg("_logUtil 触发拦截器");
        }


        //https://localhost:5001/api/TestInterceptor/GetService4?cc="测试"
        [HttpGet]
        public virtual string GetService4(string cc)
        {
            return _logUtil.ShowMsg("_logUtil 触发拦截器，触发两次拦截器");
        }
    }
}
