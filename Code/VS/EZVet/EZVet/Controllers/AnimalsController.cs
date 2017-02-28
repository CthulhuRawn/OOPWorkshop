using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EZVet.DTOs;
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
        public List<Animal> MyAnimals()
        {
            var cookieValue = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var userId = int.Parse(cookieValue[0]);

            if (cookieValue[1] == Consts.Roles.Owner)
            {
                return _animalsQueryProcessor.SearchMine(userId).ToList();
            }
            return _animalsQueryProcessor.SearchPatients(userId).ToList();
        }

        [HttpGet]
        [Route("api/animals/myPatients")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Doctor)]
        public List<Animal> MyPatients(int id)
        {
            return _animalsQueryProcessor.SearchPatients(id).ToList();
        }

        [HttpGet]
        [Route("api/animals/animal")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public Animal Animal(int id)
        {
            return _animalsQueryProcessor.GetAnimal(id);
        }

        [HttpPost]
        [Route("api/animals/animal")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public Animal Animal(Animal animal)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _animalsQueryProcessor.Save(animal, userId);
        }
    }
}