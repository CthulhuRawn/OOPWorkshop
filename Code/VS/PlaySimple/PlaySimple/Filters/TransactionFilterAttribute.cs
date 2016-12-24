using Domain;
using PlaySimple.Common;
using System;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Http;
using NHibernate;

namespace PlaySimple.Filters
{
    public class TransactionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var session = (ISession)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISession));

            session.BeginTransaction();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var session = (ISession)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISession));

            session.Transaction.Commit();
        }
    }
}