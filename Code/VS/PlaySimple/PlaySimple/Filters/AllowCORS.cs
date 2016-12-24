using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;

namespace PlaySimple.Filters
{
    public class AllowCORSFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,OPTIONS");
        }
    }
}