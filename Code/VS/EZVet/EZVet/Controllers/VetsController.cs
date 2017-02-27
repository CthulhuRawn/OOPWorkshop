using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    public class VetsController : ApiController
    {
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        private readonly IAnimalsQueryProcessor _animalsQueryProcessor;

        public VetsController(IDoctorsQueryProcessor doctorsQueryProcessor, IAnimalsQueryProcessor animalsQueryProcessor)
        {
            _doctorsQueryProcessor = doctorsQueryProcessor;
            _animalsQueryProcessor = animalsQueryProcessor;
        }

        [HttpGet]
        [Route("api/vets/all")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner)]
        public List<Doctor> All(string firstName = null, string lastName = null, string address = null, int? id = null)
        {
            return _doctorsQueryProcessor.Search(firstName, lastName, address, id).ToList();
        }

        [HttpGet]
        [Route("api/vets/assign")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner)]
        [TransactionFilter]
        public void Assign(int vetId, int petId)
        {
            _animalsQueryProcessor.AttachToDoctor(vetId, petId);
        }

        [HttpGet]
        [Route("api/vets/get")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public Doctor Get(int? vetId)
        {
            if(vetId.HasValue)
            return _doctorsQueryProcessor.GetDoctor(vetId.Value);

            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');

            int vetIdFromCookie;
            if (cookieValue[1] == Consts.Roles.Doctor && int.TryParse(cookieValue[0], out vetIdFromCookie))
            {
               return _doctorsQueryProcessor.GetDoctor(vetIdFromCookie);
            }

            return new Doctor();
        }

        [HttpPost]
        [Route("api/vets/save")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public Doctor Save(Doctor doctor)
        {
            return _doctorsQueryProcessor.Update(doctor.Id.Value, doctor);
        }

        [HttpPost]
        [Route("api/vets/saveRecommendation")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner)]
        [TransactionFilter]
        public Doctor SaveRecommendation([FromBody]Recommendation recommendation, [FromUri]int id)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _doctorsQueryProcessor.AddRecommendation(recommendation.Text, id, userId);
        }
    }
}