using System.ComponentModel.DataAnnotations;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Employee : Entity<DTOs.Employee, Domain.Employee>
    {
        [MaxLength(20)]
        public virtual string Username { get; set; }

        // TODO enable
        //[RegularExpression(@"^[a-zA-Z0-9]+$")]
        //[MinLength(8)]
        public virtual string Password { get; set; }

        [MaxLength(20)]
        public virtual string FirstName { get; set; }

        [MaxLength(20)]
        public virtual string LastName { get; set; }

        [Above(0)]
        public virtual int Salary { get; set; }

        [EmailAddress]
        public virtual string Email { get; set; }

        public override Employee Initialize(Domain.Employee domain)
        {
           Id = domain.Id;
           Password = domain.Password;
           FirstName = domain.FirstName;
           LastName = domain.LastName;
           Salary = domain.Salary;
           Email = domain.Email;

            return this;
        }
    }
}
