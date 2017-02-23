using System.Collections.Generic;
using System.Linq;
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
    }
}