using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL;
using DTO;
using DTO.Enums;
using EZVet.Filters;

namespace EZVet.Controllers
{
    public class VetsController : ApiController
    {
        private readonly IDoctorsDao _doctorsDao;
        private readonly IAnimalsDao _animalsDao;

        public VetsController(IDoctorsDao doctorsDao, IAnimalsDao animalsDao)
        {
            _doctorsDao = doctorsDao;
            _animalsDao = animalsDao;
        }

        [HttpGet]
        [Route("api/vets/all")]
        [AuthorizeRoles(  Roles.Owner)]
        public List<Doctor> All(string firstName = null, string lastName = null, string address = null, int? id = null)
        {
            return _doctorsDao.Search(firstName, lastName, address, id).ToList();
        }

        [HttpPut]
        [Route("api/vets/assign")]
        [AuthorizeRoles(  Roles.Owner)]
        [TransactionFilter]
        public void Assign(int vetId, int petId)
        {
            _animalsDao.AttachToDoctor(vetId, petId);
        }

        [HttpGet]
        [AuthorizeRoles(  Roles.Owner,Roles.Doctor)]
        public Doctor Get(int? vetId)
        {
            if(vetId.HasValue)
            return _doctorsDao.GetDoctor(vetId.Value);

            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');

            int vetIdFromCookie;
            if (cookieValue[1] == Roles.Doctor.ToString() && int.TryParse(cookieValue[0], out vetIdFromCookie))
            {
               return _doctorsDao.GetDoctor(vetIdFromCookie);
            }

            return new Doctor();
        }

        [HttpPost]
        [AuthorizeRoles(  Roles.Doctor)]
        [TransactionFilter]
        public Doctor Save(Doctor doctor)
        {
            return _doctorsDao.Update(doctor.Id.Value, doctor);
        }

        [HttpPost]
        [Route("api/vets/saveRecommendation")]
        [AuthorizeRoles(  Roles.Owner)]
        [TransactionFilter]
        public Doctor SaveRecommendation([FromBody]Recommendation recommendation, [FromUri]int id)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _doctorsDao.AddRecommendation(recommendation.Text, id, userId);
        }
    }
}