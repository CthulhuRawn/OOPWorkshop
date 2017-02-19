using Domain;
using LinqKit;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using EZVet.Common;
using EZVet.Filters;


namespace EZVet.QueryProcessors
{
    public interface IAnimalsQueryProcessor
    {
        List<DTOs.Animal> SearchMine(int id);
    }


    public class AnimalsQueryProcessor : DBAccessBase<Animal>, IAnimalsQueryProcessor
    {
        private readonly IOwnersQueryProcessor _ownersQueryProcessor;
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        public AnimalsQueryProcessor(ISession session, IOwnersQueryProcessor ownersQueryProcessor, IDoctorsQueryProcessor doctorsQueryProcessor) : base(session)
        {
            _ownersQueryProcessor = ownersQueryProcessor;
            _doctorsQueryProcessor = doctorsQueryProcessor;
        }


        public List<DTOs.Animal> SearchMine(int id)
        {
            if (_doctorsQueryProcessor.ExistsById(id))
                return
                    Query()
                        .Where(x => x.Doctors.Any(doc => doc.Id == id))
                        .Select(x => new DTOs.Animal().Initialize(x))
                        .ToList();

            if (_ownersQueryProcessor.ExistsById(id))
                return Query()
                    .Where(x => x.Owner.Id == id)
                    .Select(x => new DTOs.Animal().Initialize(x))
                    .ToList();

            return new List<DTOs.Animal>();
        }

    }
}