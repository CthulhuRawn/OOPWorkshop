using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.Daos;

namespace EZVet.Controllers
{
    public class ProfileController : ApiController
    {
        private readonly IDoctorsDao _doctorsDao;
        private readonly IOwnersDao _ownersDao;

        public ProfileController(IDoctorsDao doctorsDao,
            IOwnersDao ownersDao)
        {
            _doctorsDao = doctorsDao;
            _ownersDao = ownersDao;
        }

        [HttpPost]
        [Route("api/profile/update")]
        [Authorize(Roles =  Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public void Update(PersonLogin updateData)
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);
            switch (cookieValue[1])
            {
                case Consts.Roles.Doctor:
                    _doctorsDao.UpdateProfile(userId, updateData);
                    break;
                case Consts.Roles.Owner:
                    _ownersDao.UpdateProfile(userId, updateData);
                    break;
            }
        }

        [HttpGet]
        [Route("api/profile/get")]
        [Authorize(Roles =  Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public object Get()
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);
            return cookieValue[1] == Consts.Roles.Doctor ? (object)new Doctor().Initialize(_doctorsDao.Get(userId))
                : new Owner().Initialize(_ownersDao.Get(userId));

        }
    }
}