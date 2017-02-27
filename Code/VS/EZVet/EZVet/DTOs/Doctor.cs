using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Doctor : Entity<Doctor, Domain.Doctor>
    {
        [Key]
        [EmailAddress]
        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        [MaxLength(20)]
        public virtual string FirstName { get; set; }

        [MaxLength(20)]
        public virtual string LastName { get; set; }

        [NotInFuture]
        public virtual DateTime BirthDate { get; set; }

        public virtual Address Address { get; set; }
        public virtual string DoctorCode { get; set; }
        public virtual string AddressToDispaly { get; set; }
        public virtual  string Notes { get; set; }
        public virtual string OpeningHours { get; set; }
        public virtual string Phone { get; set; }
        public virtual IList<int> Types { get; set; }
        public virtual IList<Recommendation> Recommendations { get; set; }

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
            Types = domain.AnimalTypes.Select(x => x.Id).ToList();
            Recommendations = domain.Recommendations.Select(x => new Recommendation().Initialize(x)).ToList();

            return this;
        }
    }
}
