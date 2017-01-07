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
        private ICustomersQueryProcessor _customersQueryProcessor;
        ISession _session;

        public LoginController(ISession session, ICustomersQueryProcessor customersQueryProcessor)
        {
            _session = session;
            _customersQueryProcessor = customersQueryProcessor;
        }

        [HttpPost]
        [Route("api/login/login")]
        public LoginResponse Login(UserCredentials credentials)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(credentials.Username + ":" + credentials.Password);
            string authenticationKey = "Basic " + Convert.ToBase64String(toEncodeAsBytes);
            string role = null;
            int userId = 0;

            var user = _session.QueryOver<Domain.Owner>().Where(x => x.Username == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

            if (user != null)
            {
                role = Consts.Roles.Owner;
                userId = user.Id;
            }

            var employee = _session.QueryOver<Doctor>().Where(x => x.Username == credentials.Username && x.Password == credentials.Password).SingleOrDefault();

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
        public RegistrationReponse Registration(DTOs.Owner customer)
        {
            if (_customersQueryProcessor.Exists(customer.Email))
            {
                return new RegistrationReponse
                {
                    AlreadyExists = true
                };
            }

            

            return new RegistrationReponse
            {
                AlreadyExists = false
            };
        }
    }
}
