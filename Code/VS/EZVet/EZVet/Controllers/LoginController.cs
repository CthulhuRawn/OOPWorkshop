using System;
using System.Text;
using System.Web;
using System.Web.Http;
using DAL;
using DTO;
using DTO.Enums;
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
                role = Roles.Owner.ToString();
                userId = user.Id;
            }

            var employee = _session.QueryOver<Doctor>().Where(x => x.Email == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

            if (employee != null)
            {
                role = Roles.Doctor.ToString();
                userId = employee.Id;
            }

            role = role ?? Roles.None.ToString();

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
