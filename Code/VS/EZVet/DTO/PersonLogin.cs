using System;
using System.ComponentModel.DataAnnotations;
using DTO.Validators;

namespace DTO
{
    public class PersonLogin : EntityBase
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

        [MaxLength(15)]
        public virtual string Phone { get; set; }

        [MaxLength(30)]
        public virtual string DoctorCode { get; set; }

    }
    
}
