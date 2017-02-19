using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    public class AnimalsController : ApiController
    {
        private readonly IAnimalsQueryProcessor _animalsQueryProcessor;

        public AnimalsController(IAnimalsQueryProcessor animalsQueryProcessor)
        {
            _animalsQueryProcessor = animalsQueryProcessor;
        }

        [HttpGet]
        [Route("api/animals/myAnimals")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public List<DTOs.Animal> MyAnimals(int id)
        {
            return _animalsQueryProcessor.SearchMine(id).ToList();
        }
    }
}