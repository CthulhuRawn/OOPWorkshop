using System.Linq;
using EZVet.DTOs;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IOwnersQueryProcessor : IPersonQueryProcessor
    {
        Domain.Owner Get(int id);
        Owner Save(PersonLogin owner);
        bool Exists(string username);
        bool ExistsById(int id);
    }

    public class OwnersQueryProcessor : PersonQueryProcessor<Domain.Owner>, IOwnersQueryProcessor
    {
        public OwnersQueryProcessor(ISession session) : base(session)
        {
        }

        public bool Exists(string username)
        {
            return Query().Any(user => user.Email == username);
        }

        public bool ExistsById(int id)
        {
            return Get(id) != null;
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
    }
}