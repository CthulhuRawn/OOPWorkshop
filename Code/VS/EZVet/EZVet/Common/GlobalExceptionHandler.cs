using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http.ExceptionHandling;

namespace EZVet.Common
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            // TODO: make this work
            var ex = context.Exception;
            
            if (ex is SecurityException)
            {
                context.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User not logged in");
            }

            // catch all for unknown errors
            context.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "oops");
        }
    }
}