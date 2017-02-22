using System.Collections.Generic;
using System.Linq;
using EZVet.Common;
using EZVet.DTOs;
using LinqKit;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IOwnersQueryProcessor 
    {
        IEnumerable<Owner> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? customerId);

        Owner GetOwner(int id);
        Domain.Owner Get(int id);

        Owner Save(PersonLogin owner);

        Owner Update(int id, Owner owner);
        bool Exists(string username);
        bool ExistsById(int id);
    }

    public class OwnersQueryProcessor : DBAccessBase<Domain.Owner>, IOwnersQueryProcessor
    {
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;

        public OwnersQueryProcessor(ISession session, IDecodesQueryProcessor decodesQueryProcessor) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
        }

        public bool Exists(string username)
        {
            return Query().Any(user => user.Email == username);
        }

        public bool ExistsById(int id)
        {
            return Get(id) != null;
        }

        public IEnumerable<Owner> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? customerId)
        {
            var filter = PredicateBuilder.New<Domain.Owner>(x => true);

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
            var result = Query().Where(filter).ToList().Select(x => new Owner().Initialize(x));

            return result;
        }

        public Owner GetOwner(int id)
        {
            return new Owner().Initialize(Get(id));
        }

        public Owner Save(PersonLogin owner)
        {
            var newOwner = new Domain.Owner
            {
                FirstName = owner.FirstName,
                LastName = owner.LastName,
                Password = owner.Password,
                BirthDate = owner.BirthDate,
                Email = owner.Email,
                Address = new Domain.Address
                {
                    City = owner.Address.City,
                    Country = owner.Address.Country,
                    StreetName = owner.Address.StreetName,
                    StreetNumber = owner.Address.StreetNumber
                }
            };

            var persistedOwner = Save(newOwner);

            return new Owner().Initialize(persistedOwner);
        }

        public Owner Update(int id, Owner owner)
        {
            var existingOwner = Get(id);

            existingOwner.FirstName = owner.FirstName ?? existingOwner.FirstName;
            existingOwner.LastName = owner.LastName ?? existingOwner.LastName;
            existingOwner.Password = owner.Password ?? existingOwner.Password;
            existingOwner.Email = owner.Email ?? existingOwner.Email;

            if (owner.BirthDate != null)
                existingOwner.BirthDate = owner.BirthDate;
            
            Update(id, existingOwner);

            return new Owner().Initialize(existingOwner);
        }

    }
}