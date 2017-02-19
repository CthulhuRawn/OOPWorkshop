using System.Collections.Generic;
using Domain;
using NHibernate;
using System.Linq;
using EZVet.Common;
using LinqKit;

namespace EZVet.QueryProcessors
{
    public interface IOwnersQueryProcessor 
    {
        IEnumerable<DTOs.Owner> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? customerId);

        DTOs.Owner GetOwner(int id);

        DTOs.Owner Save(DTOs.Owner owner);

        DTOs.Owner Update(int id, DTOs.Owner owner);
        bool Exists(string username);
        bool ExistsById(int id);
    }

    public class OwnersQueryProcessor : DBAccessBase<Owner>, IOwnersQueryProcessor
    {
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;

        public OwnersQueryProcessor(ISession session, IDecodesQueryProcessor decodesQueryProcessor) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
        }

        public bool Exists(string username)
        {
            return Query().Where(user => user.Email == username).Any();
        }

        public bool ExistsById(int id)
        {
            return Query().Where(user => user.Id == id).Any();
        }

        public IEnumerable<DTOs.Owner> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? customerId)
        {
            var filter = PredicateBuilder.New<Owner>(x => true);

            if (!string.IsNullOrEmpty(firstName))
            {
                filter.And(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                filter.And(x => x.LastName.Contains(lastName));
            }

            if (minAge.HasValue)
            {
                filter.And(x => x.BirthDate <= DateUtils.GetXYearsEarly(minAge ?? 0));
            }

            if (maxAge.HasValue)
            {
                filter.And(x => x.BirthDate >= DateUtils.GetXYearsEarly(maxAge ?? 0));
            }
            
            if (customerId.HasValue)
            {
                filter.And(x => x.Id == customerId);
            }
            var result = Query().Where(filter).ToList().Select(x => new DTOs.Owner().Initialize(x));

            return result;
        }

        public DTOs.Owner GetOwner(int id)
        {
            return new DTOs.Owner().Initialize(Get(id));
        }

        public DTOs.Owner Save(DTOs.Owner owner)
        {
            var newOwner = new Owner
            {
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Password = owner.Password,
                BirthDate = owner.BirthDate,
                Email = owner.Email,
                Address =  new Address
                {
                    City = owner.Address.City,
                    Country = owner.Address.Country,
                    StreetName= owner.Address.StreetName,
                    StreetNumber= owner.Address.StreetNumber
                }
            };

            var persistedOwner = Save(newOwner);

            return new DTOs.Owner().Initialize(persistedOwner);
        }

        public DTOs.Owner Update(int id, DTOs.Owner owner)
        {
            var existingOwner = Get(id);

            existingOwner.FirstName = owner.FirstName ?? existingOwner.FirstName;
            existingOwner.LastName = owner.LastName ?? existingOwner.LastName;
            existingOwner.Password = owner.Password ?? existingOwner.Password;
            existingOwner.Email = owner.Email ?? existingOwner.Email;

            if (owner.BirthDate != null)
                existingOwner.BirthDate = owner.BirthDate;
            
            Update(id, existingOwner);

            return new DTOs.Owner().Initialize(existingOwner);
        }

    }
}