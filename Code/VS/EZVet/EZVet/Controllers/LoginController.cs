using System;
using System.Text;
using System.Web;
using System.Web.Http;
using Domain;
using EZVet.DTOs;
using EZVet.Daos;
using NHibernate;
using Doctor = Domain.Doctor;

namespace EZVet.Controllers
{
    public class LoginController : ApiController
    {
        private IOwnersDao _ownersDao;
        private IDoctorsDao _doctorsDao;
        ISession _session;

        public LoginController(ISession session, IOwnersDao ownersDao, IDoctorsDao doctorsDao)
        {
            _session = session;
            _ownersDao = ownersDao;
            _doctorsDao = doctorsDao;
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
            if (_ownersDao.Exists(entity.Email) || _doctorsDao.Exists(entity.Email))
            {
                return new RegistrationReponse
                {
                    AlreadyExists = true
                };
            }

            if (!string.IsNullOrEmpty(entity.DoctorCode))
            {
                _doctorsDao.Save(entity);
            }
            else
            {
                _ownersDao.Save(entity);
            }

            return new RegistrationReponse
            {
                AlreadyExists = false
            };
        }
    }
}
