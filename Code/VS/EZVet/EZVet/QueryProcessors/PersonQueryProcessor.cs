using Domain;
using EZVet.DTOs;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IPersonQueryProcessor
    {
        bool UpdateProfile(int id, PersonLogin entity);
    }
    public abstract class PersonQueryProcessor<T>: DBAccessBase<T> , IPersonQueryProcessor where T:Person
    {
        protected PersonQueryProcessor(ISession session) : base(session)
        {
        }
        public bool UpdateProfile(int id, PersonLogin entity)
        {
            Update(id, entity);
            return true;
        }
        
        private void Update(int id, PersonLogin entity)
        {
            var existingPerson = Get(id);
            existingPerson.Address.Country = entity.Address.Country;
            existingPerson.Address.City = entity.Address.City;
            existingPerson.Address.StreetName = entity.Address.StreetName;
            existingPerson.Address.StreetNumber = entity.Address.StreetNumber;
            existingPerson.Phone = entity.Phone;
            Update(id, existingPerson);
        }
    }
}
