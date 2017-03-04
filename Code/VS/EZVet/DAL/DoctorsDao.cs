using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using DTO;
using NHibernate;
using Doctor = DTO.Doctor;

namespace DAL
{
    public interface IDoctorsDao: IPersonDao
    {
        Doctor GetDoctor(int id);
        Domain.Doctor Get(int id);
        Doctor Save(PersonLogin doctor);
        Doctor Update(int id, Doctor doctor);
        bool Exists(string username);
        IEnumerable<Doctor> Search(string firstName, string lastName, string address, int? id);
        Doctor AddRecommendation(string recommendation, int vetId, int ownerId);
    }

    public class DoctorsDao : PersonDao<Domain.Doctor>, IDoctorsDao
    {
        private readonly IDecodesDao _decodesDao;
        private readonly IOwnersDao _ownersDao;

        public DoctorsDao(ISession session, IDecodesDao decodesDao,
            IOwnersDao ownersDao) : base(session)
        {
            _decodesDao = decodesDao;
            _ownersDao = ownersDao;
        }

        public bool Exists(string username)
        {
            return Query().Where(user => user.Email == username).Any();
        }
              
        public IEnumerable<Doctor> Search(string firstName, string lastName, string address, int? doctorId)
        {
            var filter = Query();

            if (!string.IsNullOrEmpty(firstName))
            {
                filter = filter.Where(x => x.FirstName.Contains(firstName));
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                filter = filter.Where(x => x.LastName.Contains(lastName));
            }

            if (!string.IsNullOrEmpty(address))
            {
                filter = filter.Where(
                    x =>
                        x.Address.City.Contains(address) || x.Address.StreetName.Contains(address) ||
                        x.Address.Country.Contains(address));
            }

            if (doctorId.HasValue)
            {
                filter = filter.Where(x => x.Id == doctorId);
            }
            var result = filter.ToList().Select(x => new Doctor().Initialize(x));

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
            existingDoctor.Notes = doctor.Notes ?? existingDoctor.Notes;
            existingDoctor.OpeningHours = doctor.OpeningHours ?? existingDoctor.OpeningHours;
            existingDoctor.Phone = doctor.Phone ?? existingDoctor.Phone;

            var savedTypes =
                _decodesDao.Query<AnimalTypeDecode>()
                    .Where(x => doctor.Types.Any(type => type == x.Id))
                    .ToList();
            existingDoctor.AnimalTypes = savedTypes;

            if (doctor.BirthDate != null)
                existingDoctor.BirthDate = doctor.BirthDate;

            Update(id, existingDoctor);

            return new Doctor().Initialize(existingDoctor);
        }

        public Doctor AddRecommendation(string recommendation, int vetId, int ownerId)
        {
            var existingDoctor = Get(vetId);
            existingDoctor.Recommendations.Add(new Domain.Recommendation
            {
                Text = recommendation,
                Owner = _ownersDao.Get(ownerId),
                Date = DateTime.UtcNow
            });
            return new Doctor().Initialize(Save(existingDoctor));
        }
    }
}