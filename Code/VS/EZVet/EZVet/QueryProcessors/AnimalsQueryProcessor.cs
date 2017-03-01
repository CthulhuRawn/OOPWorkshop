﻿using System.Collections.Generic;
using System.Linq;
using Domain;
using NHibernate;
using Animal = EZVet.DTOs.Animal;

namespace EZVet.QueryProcessors
{
    public interface IAnimalsQueryProcessor
    {
        Animal GetAnimal(int id);

        Animal Save(Animal animal, int cotnactId);
        Domain.Animal Get(int animalId);
        void AttachToDoctor(int vetId, int petId);
        void Update(int animalId, Domain.Animal animal);
        IQueryable<Domain.Animal> Query();
        List<Animal> Search(int doctorId, int ownerId, int? id, string animalName, string doctorName, string ownerName, int? type, int? gender);
    }


    public class AnimalsQueryProcessor : DBAccessBase<Domain.Animal>, IAnimalsQueryProcessor
    {
        private readonly IOwnersQueryProcessor _ownersQueryProcessor;
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;
        public AnimalsQueryProcessor(ISession session, IOwnersQueryProcessor ownersQueryProcessor, IDoctorsQueryProcessor doctorsQueryProcessor, IDecodesQueryProcessor decodesQueryProcessor) : base(session)
        {
            _ownersQueryProcessor = ownersQueryProcessor;
            _doctorsQueryProcessor = doctorsQueryProcessor;
            _decodesQueryProcessor = decodesQueryProcessor;
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
            
            domainAnimal.Gender = _decodesQueryProcessor.Get<GenderDecode>(animal.Gender);
            domainAnimal.Name = animal.Name;
            domainAnimal.Notes = animal.Notes;
            domainAnimal.Type = _decodesQueryProcessor.Get<AnimalTypeDecode>(animal.Type);
            domainAnimal.Weight = animal.Weight;
            domainAnimal.ChipNumber = animal.ChipNumber;
            domainAnimal.Color = animal.Color;
            domainAnimal.DateOfBirth = animal.DateOfBirth;

            if (domainAnimal.Owner == null)
            {
                domainAnimal.Owner = _ownersQueryProcessor.Get(contactId);
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
            animal.Doctor = _doctorsQueryProcessor.Get(vetId);
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
