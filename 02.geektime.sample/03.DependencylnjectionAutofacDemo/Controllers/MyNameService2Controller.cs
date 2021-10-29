using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _03.DependencylnjectionAutofacDemo.Service;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _03.DependencylnjectionAutofacDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyNameService2Controller : ControllerBase
    {


        private readonly MyNameService2 _myNameService2;

        public MyNameService2Controller(MyNameService2 myNameService2)
        {
            _myNameService2 = myNameService2;
        }

        /*
            在注册对象的同时启用 EnableClassInterceptors() 方法。
     对于以类方式的注入，Autofac Interceptor 要求类的方法为必须为 virtual 方法。
     值得注意的是：对于 子类，重写（override）父类的虚方法时，能应用到拦截器。父类可在 IoC 中注册也可不需要注册，但子类必须在 IoC 中注册（对于类的拦截器，类都必须要注册，当然，拦截器也必须要注册）。
         */
        [HttpGet]
        public string GetService5()
        {
            return _myNameService2.ShowMsg("允许在Class上使用拦截器");
        }
    }
}
