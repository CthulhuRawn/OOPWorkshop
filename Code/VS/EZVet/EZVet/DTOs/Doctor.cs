using System;
using System.ComponentModel.DataAnnotations;
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

        public override Doctor Initialize(Domain.Doctor domain)
        {
            Id = domain.Id;
            Password = domain.Password;
            FirstName = domain.FirstName;
            LastName = domain.LastName;
            BirthDate = domain.BirthDate;
            Email = domain.Email;
            Address = new Address().Initialize(domain.Address);

            return this;
        }
    }
}
