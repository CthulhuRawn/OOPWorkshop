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
        public List<Animal> MyAnimals(int? id = null, string animalName = "", string doctorName = "", string ownerName = "", int? type = null, int? gender = null)
        {
            var cookieValues = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var doctorId = -1;
            var ownerId = -1;
            if (cookieValues[1] == Consts.Roles.Doctor)
            {
                doctorId = int.Parse(cookieValues[0]);
            }
            else
            {
                ownerId = int.Parse(cookieValues[0]);
            }
            return _animalsQueryProcessor.Search(doctorId, ownerId, id, animalName, doctorName, ownerName, type, gender).ToList();
            
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