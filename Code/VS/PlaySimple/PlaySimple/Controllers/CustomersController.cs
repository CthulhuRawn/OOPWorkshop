using PlaySimple.Filters;
using PlaySimple.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http;

namespace PlaySimple.Controllers
{
    class UserTypeComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            return x.Type == y.Type && x.Value == y.Value;
        }

        public int GetHashCode(Claim obj)
        {
            return obj.GetHashCode();
        }
    }

    public class CustomersController : ApiController
    {
        private readonly ICustomersQueryProcessor _customersQueryProcessor;

        private readonly IReviewsQueryProcessor _reviewsQueryProcessor;

        private readonly IComplaintsQueryProcessor _complaintsQueryProcessor;

        private readonly UserTypeComparer _userTypeComparer;

        public CustomersController(ICustomersQueryProcessor customerQueryProcessor, IReviewsQueryProcessor reviewsQueryProcessor, 
            IComplaintsQueryProcessor complaintsQueryProcessor)
        {
            _customersQueryProcessor = customerQueryProcessor;
            _reviewsQueryProcessor = reviewsQueryProcessor;
            _complaintsQueryProcessor = complaintsQueryProcessor;

            _userTypeComparer = new UserTypeComparer();
        }

        [HttpGet]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Employee + "," + Consts.Roles.Customer)]
        public IEnumerable<DTOs.Customer> Search(string firstName = null, string lastName = null, int? minAge = null, int? maxAge = null, int? regionId = null, int? customerId = null)
        {
            return _customersQueryProcessor.Search(firstName, lastName, minAge, maxAge, regionId, customerId);
        }

        [HttpGet]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Employee + "," + Consts.Roles.Customer)]
        public DTOs.Customer Get(int id)
        {
            return _customersQueryProcessor.GetCustomer(id);
        }

        [HttpPost]
        [TransactionFilter]
        public DTOs.Customer Save([FromBody]DTOs.Customer customer)
        {
            return _customersQueryProcessor.Save(customer);
        }

        [HttpPut]
        [TransactionFilter]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Customer)]
        public DTOs.CustomerUpdateResponse Update(int id, [FromBody]DTOs.Customer customer)
        {
            string authenticationKey = null;

            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;

            if (currIdentity.Claims.Contains(new Claim(ClaimTypes.Role, Consts.Roles.Customer), _userTypeComparer))
            { 
                byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(customer.Username + ":" + customer.Password);
                authenticationKey = "Basic " + Convert.ToBase64String(toEncodeAsBytes);
            }

            return new DTOs.CustomerUpdateResponse
            {
                Customer = _customersQueryProcessor.Update(id, customer),
                AuthenticationKey = authenticationKey
            };
        }
    }
}
