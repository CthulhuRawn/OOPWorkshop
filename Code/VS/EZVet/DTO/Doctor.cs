using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DTO.Validators;

namespace DTO
{
    public class Doctor : Entity<Doctor, Domain.Doctor>
    {
        [Key]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [MaxLength(20)]
        public string FirstName { get; set; }

        [MaxLength(20)]
        public string LastName { get; set; }

        [NotInFuture]
        public DateTime BirthDate { get; set; }

        public Address Address { get; set; }
        public string DoctorCode { get; set; }
        public string AddressToDispaly { get; set; }
        public  string Notes { get; set; }
        public string OpeningHours { get; set; }
        public string Phone { get; set; }
        public IList<int> Types { get; set; }
        public IList<Recommendation> Recommendations { get; set; }

        public override Doctor Initialize(Domain.Doctor domain)
        {
            Id = domain.Id;
            Password = domain.Password;
            FirstName = domain.FirstName;
            LastName = domain.LastName;
            BirthDate = domain.BirthDate;
            Email = domain.Email;
            DoctorCode = domain.DoctorCode;
            Address = new Address().Initialize(domain.Address);
            AddressToDispaly = Address.ForUI();
            Notes = domain.Notes;
            OpeningHours = domain.OpeningHours;
            Phone = domain.Phone;
            Types = domain.AnimalTypes?.Select(x => x.Id).ToList();
            Recommendations = domain.Recommendations?.Select(x => new Recommendation().Initialize(x)).ToList();

            return this;
        }
    }
}
