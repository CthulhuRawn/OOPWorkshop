using Domain;
using DTO;
using NHibernate;

namespace DAL
{
    public interface IPersonDao
    {
        bool UpdateProfile(int id, PersonLogin entity);
    }
    public abstract class PersonDao<T>: DBAccessBase<T> , IPersonDao where T:Person
    {
        protected PersonDao(ISession session) : base(session)
        {
        }
        public bool UpdateProfile(int id, PersonLogin entity)
        {
            var existingPerson = Get(id);
            existingPerson.Address.Country = entity.Address.Country;
            existingPerson.Address.City = entity.Address.City;
            existingPerson.Address.StreetName = entity.Address.StreetName;
            existingPerson.Address.StreetNumber = entity.Address.StreetNumber;
            existingPerson.Phone = entity.Phone;
            Update(id, existingPerson);
            return true;
        }
    }
}
