using System.Collections.Generic;
using System.Linq;
using EZVet.Common;
using EZVet.DTOs;
using LinqKit;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IDoctorsQueryProcessor
    {
        IEnumerable<Doctor> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? doctorId);

        Doctor GetDoctor(int id);

        Doctor Save(PersonLogin doctor);

        Doctor Update(int id, Doctor doctor);
        bool Exists(string username);
        bool ExistsById(int id);
    }

    public class DoctorsQueryProcessor : DBAccessBase<Domain.Doctor>, IDoctorsQueryProcessor
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

        public IEnumerable<Doctor> Search(string firstName, string lastName, int? minAge, int? maxAge, int? region, int? doctorId)
        {
            var filter = PredicateBuilder.New<Domain.Doctor>(x => true);

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
            var result = Query().Where(filter).ToList().Select(x => new Doctor().Initialize(x));

            return result;
        }

        public Doctor GetDoctor(int id)
        {
            return new Doctor().Initialize(Get(id));
        }

        public Doctor Save(PersonLogin doctor)
        {
            var newDoctor = new Domain.Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Password = doctor.Password,
                BirthDate = doctor.BirthDate,
                Email = doctor.Email,
                DoctorCode = doctor.DoctorCode,
                Address = new Domain.Address
                {
                    City = doctor.Address.City,
                    Country = doctor.Address.Country,
                    StreetName = doctor.Address.StreetName,
                    StreetNumber = doctor.Address.StreetNumber
                }
            };

            var persistedDoctor = Save(newDoctor);

            return new Doctor().Initialize(persistedDoctor);
        }

        public Doctor Update(int id, Doctor doctor)
        {
            var existingDoctor = Get(id);

            existingDoctor.FirstName = doctor.FirstName ?? existingDoctor.FirstName;
            existingDoctor.LastName = doctor.LastName ?? existingDoctor.LastName;
            existingDoctor.Password = doctor.Password ?? existingDoctor.Password;
            existingDoctor.Email = doctor.Email ?? existingDoctor.Email;

            if (doctor.BirthDate != null)
                existingDoctor.BirthDate = doctor.BirthDate;
            
            Update(id, existingDoctor);

            return new Doctor().Initialize(existingDoctor);
        }

    }
}