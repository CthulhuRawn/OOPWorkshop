using System.Collections.Generic;
using System.Linq;
using Domain;
using NHibernate;
using Animal = EZVet.DTOs.Animal;

namespace EZVet.QueryProcessors
{
    public interface IAnimalsQueryProcessor
    {
        List<Animal> SearchMine(int id);

        Animal GetAnimal(int id);

        Animal Save(Animal animal, int cotnactId);
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


        public List<Animal> SearchMine(int id)
        {
            if (_doctorsQueryProcessor.ExistsById(id))
                return
                    Query()
                        .Where(x => x.Doctors.Any(doc => doc.Id == id))
                        .Select(x => new Animal().Initialize(x))
                        .ToList();

            if (_ownersQueryProcessor.ExistsById(id))
                return Query()
                    .Where(x => x.Owner.Id == id)
                    .Select(x => new Animal().Initialize(x))
                    .ToList();

            return new List<Animal>();
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
    }
}
