using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EZVet.DTOs
{
    public class Vaccine : Entity<Vaccine, Domain.Treatment>
    {
        [MaxLength(30)]
        public virtual string Name { get; set; }
        public virtual DateTime Date { get; set; }

        public override Vaccine Initialize(Domain.Treatment domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Date = domain.ContainingTreatment.Date;
            

            return this;
        }
    }
}
