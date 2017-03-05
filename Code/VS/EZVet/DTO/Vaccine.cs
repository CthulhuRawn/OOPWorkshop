using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Vaccine : Entity<Vaccine, Domain.Treatment>
    {
        [MaxLength(30)]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }

        public override Vaccine Initialize(Domain.Treatment domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Price = domain.Price;
            Date = domain.ContainingTreatment.Date;
            

            return this;
        }
    }
}
