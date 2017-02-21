using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EZVet.DTOs
{
    public class Treatment : Entity<Treatment, Domain.TreatmentReport>
    {
        [MaxLength(30)]
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual DateTime Date { get; set; }

        public override Treatment Initialize(Domain.TreatmentReport domain)
        {
            Id = domain.Id;
            Name = string.Join(",", domain.Treatments.Select(x => x.Name).ToArray());
            Price = domain.TotalPrice;
            Date = domain.Date;
            

            return this;
        }
    }
}
