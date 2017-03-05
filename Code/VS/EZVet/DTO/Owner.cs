using System;
using System.ComponentModel.DataAnnotations;
using DTO.Validators;

namespace DTO
{
    public class Owner : Entity<Owner, Domain.Owner>
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
        public string Phone{ get; set; }

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
