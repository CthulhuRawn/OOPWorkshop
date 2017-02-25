using System;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Domain;
using EZVet.DTOs;
using EZVet.QueryProcessors;
using NHibernate;
using Doctor = Domain.Doctor;
using Owner = EZVet.DTOs.Owner;

namespace EZVet.Controllers
{
    public class LoginController : ApiController
    {
        private IOwnersQueryProcessor _ownersQueryProcessor;
        private IDoctorsQueryProcessor _doctorsQueryProcessor;
        ISession _session;

        public LoginController(ISession session, IOwnersQueryProcessor ownersQueryProcessor, IDoctorsQueryProcessor doctorsQueryProcessor)
        {
            _session = session;
            _ownersQueryProcessor = ownersQueryProcessor;
            _doctorsQueryProcessor = doctorsQueryProcessor;
        }

        [HttpPost]
        [Route("api/login/login")]
        public LoginResponse Login(UserCredentials credentials)
        {
            var toEncodeAsBytes = Encoding.ASCII.GetBytes(credentials.Username + ":" + credentials.Password);
            var authenticationKey = "Basic " + Convert.ToBase64String(toEncodeAsBytes);
            string role = null;
            var userId = 0;

            var user = _session.QueryOver<Domain.Owner>().Where(x => x.Email == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

            if (user != null)
            {
                role = Consts.Roles.Owner;
                userId = user.Id;
            }

            var employee = _session.QueryOver<Doctor>().Where(x => x.Email == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

            if (employee != null)
            {
                role = Consts.Roles.Doctor;
                userId = employee.Id;
            }

            var admin = _session.QueryOver<Admin>().Where(x => x.Username == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

            if (admin != null)
            {
                role = Consts.Roles.Admin;
                userId = admin.Id;
            }

            role = role ?? Consts.Roles.None;

            var cookie = new HttpCookie("userId", userId + ":" + role);
            cookie.Expires = DateTime.MaxValue;
            cookie.Domain = Request.RequestUri.Host;
            cookie.Path = "/";

            HttpContext.Current.Response.Cookies.Add(cookie);

            return new LoginResponse
            {
                UserId = userId,
                AuthorizationKey = authenticationKey,
                Role = role
            };
        }

        [HttpPost]
        [Route("api/login/registration")]
        public RegistrationReponse Registration(PersonLogin entity)
        {
            if (_ownersQueryProcessor.Exists(entity.Email) || _doctorsQueryProcessor.Exists(entity.Email))
            {
                return new RegistrationReponse
                {
                    AlreadyExists = true
                };
            }

            if (!string.IsNullOrEmpty(entity.DoctorCode))
            {
                _doctorsQueryProcessor.Save(entity);
            }
            else
            {
                _ownersQueryProcessor.Save(entity);
            }

            return new RegistrationReponse
            {
                AlreadyExists = false
            };
        }
    }
}
