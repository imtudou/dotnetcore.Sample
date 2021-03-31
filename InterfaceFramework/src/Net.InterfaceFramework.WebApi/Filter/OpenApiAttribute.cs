using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NetCore.IntefaceFramework.WebApi.Filter
{
    public class OpenApiAttribute : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            string actionname = actionContext.Request.ToString();

        }



    }
}
