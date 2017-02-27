using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        private readonly IOwnersQueryProcessor _ownersQueryProcessor;

        public ProfileController(IDoctorsQueryProcessor doctorsQueryProcessor,
            IOwnersQueryProcessor ownersQueryProcessor)
        {
            _doctorsQueryProcessor = doctorsQueryProcessor;
            _ownersQueryProcessor = ownersQueryProcessor;
        }

        [HttpPost]
        [Route("api/profile/update")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public void Update(PersonLogin updateData)
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);
            switch (cookieValue[1])
            {
                case Consts.Roles.Doctor:
                    _doctorsQueryProcessor.UpdateProfile(userId, updateData);
                    break;
                case Consts.Roles.Owner:
                    _ownersQueryProcessor.UpdateProfile(userId, updateData);
                    break;
            }
        }

        [HttpGet]
        [Route("api/profile/get")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public object Get()
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);
            return cookieValue[1] == Consts.Roles.Doctor ? (object)new Doctor().Initialize(_doctorsQueryProcessor.Get(userId))
                : new Owner().Initialize(_ownersQueryProcessor.Get(userId));

        }
    }
}