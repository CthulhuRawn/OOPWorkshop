using System;
using System.ComponentModel.DataAnnotations;

namespace EZVet.DTOs
{
    public class Treatment : Entity<Treatment, Domain.Treatment>
    {
        [MaxLength(30)]
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual DateTime Date { get; set; }

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
