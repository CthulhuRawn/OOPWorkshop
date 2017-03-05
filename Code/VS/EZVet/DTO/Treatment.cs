using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Treatment : Entity<Treatment, Domain.Treatment>
    {
        [MaxLength(30)]
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime Date { get; set; }

        public override Treatment Initialize(Domain.Treatment domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Price = domain.Price;
            Date = domain.ContainingTreatment.Date;
            

            return this;
        }
    }
}
