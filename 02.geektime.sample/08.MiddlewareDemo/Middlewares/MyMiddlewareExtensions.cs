using Microsoft.AspNetCore.Builder;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _08.MiddlewareDemo.Middlewares
{
    public static class MyMiddlewareExtensions
    {
        /// <summary>
        /// 自定义中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MyMiddleware>();
        }

        /// <summary>
        /// 自定义中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder app,Action<MyMiddlewareOptions> action)
        {
            return app.UseMiddleware<MyMiddleware>(action);
        }
    }
}
