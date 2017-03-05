using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Medication : Entity<Medication, Domain.Treatment>
    {
        [MaxLength(30)]
        public string Name { get; set; }
        public string Dose { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }

        public override Medication Initialize(Domain.Treatment domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Date = domain.ContainingTreatment.Date;
            Price = domain.Price;
            Dose = domain.Dose;

            return this;
        }
    }
}
