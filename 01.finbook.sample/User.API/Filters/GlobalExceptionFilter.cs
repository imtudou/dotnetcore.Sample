using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<GlobalExceptionFilter> _logger;
        
        public GlobalExceptionFilter(IHostEnvironment hostEnvironment,ILogger<GlobalExceptionFilter> logger)
        {

            _hostEnvironment = hostEnvironment;
            _logger = logger;
        
        }
        public void OnException(ExceptionContext context)
        {

            var json = new JsonErropResponse();
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.Message = context.Exception.Message;
                context.Result = new BadRequestObjectResult(json);
            }
            else 
            {
                json.Message = "内部未知错误！";
                context.Result = new InternalServerErrorObjectResult(json);
            }

            //开发环境
            if (_hostEnvironment.IsDevelopment())
            {
                json.DeveloperMessage = context.Exception.StackTrace;
            }


            _logger.LogError(context.Exception, context.Exception.Message);
            context.ExceptionHandled = true;

        }
    }



    public class InternalServerErrorObjectResult : ObjectResult 
    {

        public InternalServerErrorObjectResult(object error) : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }

    }
}
