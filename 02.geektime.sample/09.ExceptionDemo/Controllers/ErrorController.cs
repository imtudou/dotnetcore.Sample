using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using _09.ExceptionDemo.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _09.ExceptionDemo
{
    [Route("/error")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var ex = exceptionHandlerPathFeature?.Error;

            
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var error = exceptionFeature?.Error as IMyException;
            if (error == null)
            {
                error = MyException.Unknown;
            }
            return View(error);
        }
    }
}
