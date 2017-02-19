using System.Collections.Generic;
using Domain;
using NHibernate;
using System.Linq;
using EZVet.Common;
using LinqKit;

namespace EZVet.QueryProcessors
{
    public interface IDoctorsQueryProcessor
    {
        IEnumerable<DTOs.Doctor> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? doctorId);

        DTOs.Doctor GetDoctor(int id);

        DTOs.Doctor Save(DTOs.Doctor doctor);

        DTOs.Doctor Update(int id, DTOs.Doctor doctor);
        bool Exists(string username);
        bool ExistsById(int id);
    }

    public class DoctorsQueryProcessor : DBAccessBase<Doctor>, IDoctorsQueryProcessor
    {
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;

        public DoctorsQueryProcessor(ISession session, IDecodesQueryProcessor decodesQueryProcessor) : base(session)
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

        public IEnumerable<DTOs.Doctor> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? doctorId)
        {
            var filter = PredicateBuilder.New<Doctor>(x => true);

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
            
            if (doctorId.HasValue)
            {
                filter.And(x => x.Id == doctorId);
            }
            var result = Query().Where(filter).ToList().Select(x => new DTOs.Doctor().Initialize(x));

            return result;
        }

        public DTOs.Doctor GetDoctor(int id)
        {
            return new DTOs.Doctor().Initialize(Get(id));
        }

        public DTOs.Doctor Save(DTOs.Doctor doctor)
        {
            var newDoctor = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Password = doctor.Password,
                BirthDate = doctor.BirthDate,
                Email = doctor.Email,
                Address =  new Address
                {
                    City = doctor.Address.City,
                    Country = doctor.Address.Country,
                    StreetName= doctor.Address.StreetName,
                    StreetNumber= doctor.Address.StreetNumber
                }
            };

            var persistedDoctor = Save(newDoctor);

            return new DTOs.Doctor().Initialize(persistedDoctor);
        }

        public DTOs.Doctor Update(int id, DTOs.Doctor doctor)
        {
            var existingDoctor = Get(id);

            existingDoctor.FirstName = doctor.FirstName ?? existingDoctor.FirstName;
            existingDoctor.LastName = doctor.LastName ?? existingDoctor.LastName;
            existingDoctor.Password = doctor.Password ?? existingDoctor.Password;
            existingDoctor.Email = doctor.Email ?? existingDoctor.Email;

            if (doctor.BirthDate != null)
                existingDoctor.BirthDate = doctor.BirthDate;
            
            Update(id, existingDoctor);

            return new DTOs.Doctor().Initialize(existingDoctor);
        }

    }
}