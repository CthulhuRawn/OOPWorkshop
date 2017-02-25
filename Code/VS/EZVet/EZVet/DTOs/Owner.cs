using System;
using System.ComponentModel.DataAnnotations;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Owner : Entity<Owner, Domain.Owner>
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
        public virtual string Phone{ get; set; }

        public override Owner Initialize(Domain.Owner domain)
        {
            Id = domain.Id;
            Password = domain.Password;
            FirstName = domain.FirstName;
            LastName = domain.LastName;
            BirthDate = domain.BirthDate;
            Email = domain.Email;
            Address = new Address().Initialize(domain.Address);
            Phone = domain.Phone;
            return this;
        }
    }
}
