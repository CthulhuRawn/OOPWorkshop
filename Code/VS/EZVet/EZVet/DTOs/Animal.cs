using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EZVet.DTOs
{
    public class Animal : Entity<DTOs.Animal, Domain.Animal>
    {
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public string Type { get; set; }
        public double Age { get; set; }
        public string Gender { get; set; }
        public DateTime NextVisit { get; set; }
       

        public override Animal Initialize(Domain.Animal domain)
        {
            var previousOrder = domain.Orders.OrderByDescending(x => x.Id).FirstOrDefault();
            Id = domain.Id;
            Name = domain.Name;
            DoctorName = previousOrder?.Doctor.FirstName + previousOrder?.Doctor.LastName;
            Type = domain.Type.ToString();
            Age = (DateTime.UtcNow - domain.DateOfBirth).Days / 365.0;
            Gender = domain.Gender.ToString();
            NextVisit = DateTime.UtcNow.AddDays(180);
            
            return this;
        }
    }
}
