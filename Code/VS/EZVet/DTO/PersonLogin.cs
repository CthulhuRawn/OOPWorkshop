using System;
using System.ComponentModel.DataAnnotations;
using DTO.Validators;

namespace DTO
{
    public class PersonLogin 
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

        [MaxLength(15)]
        public string Phone { get; set; }

        [MaxLength(30)]
        public string DoctorCode { get; set; }

    }
    
}
