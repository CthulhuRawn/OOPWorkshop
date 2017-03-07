using System.Web;
using System.Web.Http;
using DAL;
using DTO;
using DTO.Enums;
using EZVet.Filters;

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

        [HttpPut]
        [AuthorizeRoles(Roles.Owner, Roles.Doctor)]
        [TransactionFilter]
        public void Update(PersonLogin updateData)
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);
            if (cookieValue[1] == Roles.Doctor.ToString())
            {

                _doctorsDao.UpdateProfile(userId, updateData);
            }
            else
            {
                _ownersDao.UpdateProfile(userId, updateData);

            }

        }

        [HttpGet]
        [AuthorizeRoles(Roles.Owner, Roles.Doctor)]
        public object Get()
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);
            return cookieValue[1] == Roles.Doctor.ToString()
                ? (object) new Doctor().Initialize(_doctorsDao.Get(userId))
                : new Owner().Initialize(_ownersDao.Get(userId));

        }
    }
}