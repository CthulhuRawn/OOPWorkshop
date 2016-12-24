using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace PlaySimple.Common
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