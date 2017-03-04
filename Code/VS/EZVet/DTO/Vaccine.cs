using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class Vaccine : Entity<Vaccine, Domain.Treatment>
    {
        [MaxLength(30)]
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual double Price { get; set; }

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
