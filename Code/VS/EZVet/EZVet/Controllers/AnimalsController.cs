using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL;
using DTO;
using DTO.Enums;
using EZVet.Filters;
using FluentNHibernate.Utils;
using Animal = DTO.Animal;

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
        [AuthorizeRoles(Roles.Owner, Roles.Doctor)]
        public List<Animal> MyAnimals(int? id = null, string animalName = "", string doctorName = "", string ownerName = "", int? type = null, int? gender = null)
        {
            var cookieValues = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var doctorId = -1;
            var ownerId = -1;
            if (cookieValues[1] == Roles.Doctor.ToString())
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
        [AuthorizeRoles(Roles.Owner ,Roles.Doctor)]
        public Animal Get(int id)
        {
            return _animalsDao.GetAnimal(id);
        }

        // POST: api/Animals
        [HttpPost]
        [AuthorizeRoles(Roles.Owner ,Roles.Doctor)]
        [TransactionFilter]
        public Animal Save(Animal animal)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _animalsDao.Save(animal, userId);
        }
    }
}