using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _03.DependencylnjectionAutofacDemo.Service;

using Autofac.Extras.DynamicProxy;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _03.DependencylnjectionAutofacDemo.Controllers
{
    [Intercept(typeof(MyInterceptor))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogUtil _logUtil;
        public ValuesController(ILogUtil log)
        {
            _logUtil = log;
        }
        [HttpGet]
        public string Get([FromServices] IEnumerable<IMyService> myService)
        {
            foreach (var item in myService)
            {
                item.ShowCode();
            }
            return $"Hello World!  {DateTime.Now}";
        }

        [HttpGet]
        public string GetService3()
        {
            return _logUtil.ShowMsg("_logUtil 触发拦截器");
        }
    }
}
