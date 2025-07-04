using Castle.DynamicProxy;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace _03.DependencylnjectionAutofacDemo.Service
{
    /// <summary>
    /// Autofac 实现AOP
    /// 依赖包：Autofac.Extras.DynamicProxy
    /// 实现接口：IInterceptor
    /// </summary>
    public class MyInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var arguments  = invocation.Arguments;
            var invocationMethodName = invocation.Method.Name;
            var cc = invocation.InvocationTarget;

            Console.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
            invocation.Method.Name,
            string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));


            
            var result = invocation.ReturnValue = "拦截器方法执行完毕";
            if (!string.IsNullOrEmpty(result?.ToString()))
            {
                invocation.Proceed();
            }
            
            Console.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
            return;

        }
    }
}
