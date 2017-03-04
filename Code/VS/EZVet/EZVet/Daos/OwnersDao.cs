using System.Linq;
using EZVet.DTOs;
using NHibernate;

namespace EZVet.Daos
{
    public interface IOwnersDao : IPersonDao
    {
        Domain.Owner Get(int id);
        Owner Save(PersonLogin owner);
        bool Exists(string username);
    }

    public class OwnersDao : PersonDao<Domain.Owner>, IOwnersDao
    {
        public OwnersDao(ISession session) : base(session)
        {
        }

        public bool Exists(string username)
        {
            return Query().Any(user => user.Email == username);
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