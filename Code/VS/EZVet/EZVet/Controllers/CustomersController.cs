using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
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
        private readonly IOwnersQueryProcessor _customersQueryProcessor;

        private readonly IReviewsQueryProcessor _reviewsQueryProcessor;

        private readonly IComplaintsQueryProcessor _complaintsQueryProcessor;

        private readonly UserTypeComparer _userTypeComparer;

        public CustomersController(IOwnersQueryProcessor customerQueryProcessor, IReviewsQueryProcessor reviewsQueryProcessor, 
            IComplaintsQueryProcessor complaintsQueryProcessor)
        {
            _customersQueryProcessor = customerQueryProcessor;
            _reviewsQueryProcessor = reviewsQueryProcessor;
            _complaintsQueryProcessor = complaintsQueryProcessor;

            _userTypeComparer = new UserTypeComparer();
        }

        
    }
}
