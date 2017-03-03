using System;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Domain;
using EZVet.Controllers;
using NHibernate;

namespace EZVet.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GlobalAuthorizationFilter : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Checks basic authentication request
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            // don't enforce authorization for login controller.
            if (filterContext.ControllerContext.Controller is LoginController)
                return;

            var identity = FetchAuthHeader(filterContext);

            // no authentication header sent
            if (identity == null)
                throw new SecurityException();
            
            var genericPrincipal = new ClaimsPrincipal(identity);
            
            // saves the user details on the current thread
            Thread.CurrentPrincipal = genericPrincipal;
            HttpContext.Current.User = genericPrincipal;

            if (!AuthorizeUser(identity.Name, identity.Password))
            {
                throw new SecurityException();
            }

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// Virtual method.Can be overriden with the custom Authorization.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pass"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool AuthorizeUser(string username, string password)
        {
            var authorized = false;
            var session =(ISession)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ISession));

            var user = session.QueryOver<Owner>().Where(x => x.Email == username && x.Password == password).SingleOrDefault();

            if (user != null)
            { 
                AuthorizationSucceed(user.Id, Consts.Roles.Owner);
                authorized = true;
            }

            var doctor = session.QueryOver<Doctor>().Where(x => x.Email == username && x.Password == password).SingleOrDefault();

            if (doctor != null)
            {
                AuthorizationSucceed(doctor.Id, Consts.Roles.Doctor);
                authorized = true;
            }

            return authorized;
        }

        private void AuthorizationSucceed(int userId, string userRole)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;

            currIdentity.UserId = userId;
            currIdentity.AddClaim(new Claim(ClaimTypes.Role, userRole));
        }

        /// <summary>
        /// Checks for autrhorization header in the request and parses it, creates user credentials and returns as BasicAuthenticationIdentity
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string authHeaderValue = null;

            var authRequest = filterContext.Request.Headers.Authorization;

            // support only basic authentication (the browser sends username:password as a request header)
            if (authRequest != null && authRequest.Scheme == "Basic")
            {
                authHeaderValue = authRequest.Parameter;
            }

            // no header found
            if (string.IsNullOrEmpty(authHeaderValue))
                return null;

            // deserialize authentication header
            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));

            var credentials = authHeaderValue.Split(':');

            return credentials.Length < 2 ? null : new BasicAuthenticationIdentity(credentials[0], credentials[1]);
        }
    }
}