using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Text.Json;

namespace FluentValidation.Sample.Filter
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {

        private ILogger<GlobalExceptionFilter> _logger;
        //构造注入日志组件
        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            //日志收集
            _logger.LogError(context.Exception, context?.Exception?.Message ?? "异常");

            var result = new
            {
                code = HttpStatusCode.InternalServerError,
                Msg = "自定义错误：异常",
                Errors = context.Exception.Message
            };

            context.Result = new ContentResult()
            {
                Content = JsonSerializer.Serialize(result),
                ContentType = "application/json",
                StatusCode = 500
            };
        }
    }
}
