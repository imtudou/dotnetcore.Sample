using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.IntefaceFramework.WebApi.Filter
{
    public class TestApiFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            HttpRequest Request = context.HttpContext.Request;
            Request.Headers.Add("Access-Control-Allow-Origin","*");
        }
    }
}
