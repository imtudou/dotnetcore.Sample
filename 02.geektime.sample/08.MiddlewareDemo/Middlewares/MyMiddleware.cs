using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace _08.MiddlewareDemo.Middlewares
{
    public class MyMiddleware
    {

        public readonly RequestDelegate _next;
        public readonly ILogger<MyMiddleware> _logger;

        public MyMiddleware(RequestDelegate next, 
            ILogger<MyMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// 中间件是一种装配到应用管道以处理请求和相应的软件
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public async Task InvokeAsync(HttpContext context)
        {
            using (_logger.BeginScope("context.TraceIdentifier：{0}", context.TraceIdentifier))
            {
                _logger.LogInformation("开始执行");
                await _next(context);
                _logger.LogInformation("执行结束");
            }
        }
    }
}
