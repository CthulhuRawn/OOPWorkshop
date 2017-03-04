using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.Daos;

namespace EZVet.Controllers
{
    public class AnimalsController : ApiController
    {
        private readonly IAnimalsDao _animalsDao;

        public AnimalsController(IAnimalsDao animalsDao)
        {
            _animalsDao = animalsDao;
        }

        [HttpGet]
        [Route("api/animals/myAnimals")]
        [Authorize(Roles =  Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
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
            return _animalsDao.Search(doctorId, ownerId, id, animalName, doctorName, ownerName, type, gender).ToList();
            
        }

        // GET: api/Animals/5
        [HttpGet]
        [Authorize(Roles =  Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public Animal Get(int id)
        {
            return _animalsDao.GetAnimal(id);
        }

        // POST: api/Animals
        [HttpPost]
        [Authorize(Roles =  Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public Animal Save(Animal animal)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _animalsDao.Save(animal, userId);
        }
    }
}