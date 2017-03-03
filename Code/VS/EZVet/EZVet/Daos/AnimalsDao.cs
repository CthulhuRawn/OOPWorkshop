using System.Collections.Generic;
using System.Linq;
using Domain;
using NHibernate;
using Animal = EZVet.DTOs.Animal;

namespace EZVet.Daos
{
    public interface IAnimalsDao
    {
        Animal GetAnimal(int id);

        Animal Save(Animal animal, int cotnactId);
        Domain.Animal Get(int animalId);
        void AttachToDoctor(int vetId, int petId);
        void Update(int animalId, Domain.Animal animal);
        IQueryable<Domain.Animal> Query();
        List<Animal> Search(int doctorId, int ownerId, int? id, string animalName, string doctorName, string ownerName, int? type, int? gender);
    }


    public class AnimalsDao : DBAccessBase<Domain.Animal>, IAnimalsDao
    {
        private readonly IOwnersDao _ownersDao;
        private readonly IDoctorsDao _doctorsDao;
        private readonly IDecodesDao _decodesDao;
        public AnimalsDao(ISession session, IOwnersDao ownersDao, IDoctorsDao doctorsDao, IDecodesDao decodesDao) : base(session)
        {
            _ownersDao = ownersDao;
            _doctorsDao = doctorsDao;
            _decodesDao = decodesDao;
        }
        
        
        public Animal Save(Animal animal, int contactId)
        {
            Domain.Animal domainAnimal;
            if (animal.Id.HasValue && animal.Id > 0)
            {
                domainAnimal = Get(animal.Id.Value);
            }
            else
            {
                domainAnimal = new Domain.Animal();
            }
            
            domainAnimal.Gender = _decodesDao.Get<GenderDecode>(animal.Gender);
            domainAnimal.Name = animal.Name;
            domainAnimal.Notes = animal.Notes;
            domainAnimal.Type = _decodesDao.Get<AnimalTypeDecode>(animal.Type);
            domainAnimal.Weight = animal.Weight;
            domainAnimal.ChipNumber = animal.ChipNumber;
            domainAnimal.Color = animal.Color;
            domainAnimal.DateOfBirth = animal.DateOfBirth;

            if (domainAnimal.Owner == null)
            {
                domainAnimal.Owner = _ownersDao.Get(contactId);
            }


            return new Animal().Initialize(Save(domainAnimal));
        }

        public Animal GetAnimal(int id)
        {
            return new Animal().Initialize(Get(id));
        }

        public void AttachToDoctor(int vetId, int petId)
        {
            var animal = Get(petId);
            animal.Doctor = _doctorsDao.Get(vetId);
            Update(petId, animal);
        }

        public List<Animal> Search(int doctorId, int ownerId, int? id, string animalName, string doctorName, string ownerName, int? type,
            int? gender)
        {
            return
                Query()
                    .Where(x => x.Owner.Id == ownerId || x.Doctor.Id == doctorId)
                    .Where(
                        x =>
                            x.Name.Contains(animalName) && (x.Type.Id == type || type == null) && (x.Gender.Id == gender ||
                            gender == null))
                    .ToList()
                    .Where(x => x.Owner.GetName().Contains(ownerName) && x.Doctor == null || x.Doctor.GetName().Contains(doctorName))
                    .Select(x => new Animal().Initialize(x)).ToList();
        }
    }
}
