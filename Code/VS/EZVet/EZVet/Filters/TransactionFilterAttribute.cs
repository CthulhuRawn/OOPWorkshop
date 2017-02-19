using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NHibernate;

namespace EZVet.Filters
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