using System;
using System.Collections.Generic;
using Domain;
using System.ComponentModel.DataAnnotations;
using PlaySimple.Validators;

namespace PlaySimple.DTOs
{
    public class Customer : Entity<DTOs.Customer, Domain.Customer>
    {
        [MaxLength(20)]
        [Key]
        public virtual string Username { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9]+$")]
        //[MinLength(8)]
        public virtual string Password { get; set; }

        [MaxLength(20)]
        public virtual string FirstName { get; set; }

        [MaxLength(20)]
        public virtual string LastName { get; set; }

        [NotInFuture]
        public virtual DateTime BirthDate { get; set; }
        
        [EmailAddress]
        public virtual string Email { get; set; }

        [IsEnumOfType(typeof(Consts.Decodes.RegionDecode))]
        public virtual int? Region { get; set; }

        public virtual DateTime? FreezeDate { get; set; }

        public override Customer Initialize(Domain.Customer domain)
        {
            Id = domain.Id;
            Username = domain.Username;
            Password = domain.Password;
            FirstName = domain.FirstName;
            LastName = domain.LastName;
            BirthDate = domain.BirthDate;
            Email = domain.Email;
            Region = domain.Region.Id;
            FreezeDate = domain.FreezeDate;

            return this;
        }
    }
}
