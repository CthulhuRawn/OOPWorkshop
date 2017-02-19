using System.Collections.Generic;
using System.Linq;
using EZVet.DTOs;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IAnimalsQueryProcessor
    {
        List<Animal> SearchMine(int id);
    }


    public class AnimalsQueryProcessor : DBAccessBase<Domain.Animal>, IAnimalsQueryProcessor
    {
        private readonly IOwnersQueryProcessor _ownersQueryProcessor;
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        public AnimalsQueryProcessor(ISession session, IOwnersQueryProcessor ownersQueryProcessor, IDoctorsQueryProcessor doctorsQueryProcessor) : base(session)
        {
            _ownersQueryProcessor = ownersQueryProcessor;
            _doctorsQueryProcessor = doctorsQueryProcessor;
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

    }
}