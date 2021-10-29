using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Linq;

namespace FluentValidation.Sample.Filter
{
    public class ActionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.HttpContext.Request.Form.Any())
            {
                context.Result = new ContentResult()
                {
                    Content = "测试",
                    ContentType = "application/json",
                    StatusCode = 400
                };
            }
            else
            {

            }
        }

    }
}
