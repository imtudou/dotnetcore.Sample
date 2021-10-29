using Autofac.Extras.DynamicProxy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _03.DependencylnjectionAutofacDemo.Service
{
    [Intercept(typeof(MyInterceptor))]
    public interface ILogUtil
    {
        string ShowMsg(string msg);
    
    }
    public class LogUtil : ILogUtil
    {
        public string ShowMsg(string msg)
        {
            return $"ILogUtil.ShowCode：[{msg}]" + DateTime.Now;
        }
    }
}
