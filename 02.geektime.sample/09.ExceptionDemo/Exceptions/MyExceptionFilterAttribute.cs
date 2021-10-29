using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace _09.ExceptionDemo.Exceptions
{
    public class MyExceptionFilterAttribute : IExceptionFilter
    {
        private readonly ILogger<MyExceptionFilterAttribute> _logger;
        public MyExceptionFilterAttribute(ILogger<MyExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            _logger.LogInformation(context.Exception, context.Exception.Message);
            var myException = context.Exception as IMyException;
            if (myException == null)
            {
                myException = MyException.Unknown;
                context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }
            else
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            }

            context.Result = new JsonResult(myException)
            {
                ContentType = "contentType:application/json;charset:utf-8"
            };
            context.HttpContext.Response.Redirect("/error");
            return;
        }
    }
}
