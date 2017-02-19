using System;
using System.Text;
using System.Web.Http;
using Domain;
using EZVet.DTOs;
using EZVet.QueryProcessors;
using NHibernate;


namespace EZVet.Controllers
{
    public class LoginController : ApiController
    {
        private IOwnersQueryProcessor _ownersQueryProcessor;
        ISession _session;

        public LoginController(ISession session, IOwnersQueryProcessor ownersQueryProcessor)
        {
            _session = session;
            _ownersQueryProcessor = ownersQueryProcessor;
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

            var employee = _session.QueryOver<Domain.Doctor>().Where(x => x.Email == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

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

            return new LoginResponse
            {
                UserId = userId,
                AuthorizationKey = authenticationKey,
                Role = role
            };
        }

        [HttpPost]
        [Route("api/login/registration")]
        public RegistrationReponse Registration(DTOs.Owner owner)
        {
            if (_ownersQueryProcessor.Exists(owner.Email))
            {
                return new RegistrationReponse
                {
                    AlreadyExists = true
                };
            }

            _ownersQueryProcessor.Save(owner);

            return new RegistrationReponse
            {
                AlreadyExists = false
            };
        }
    }
}
