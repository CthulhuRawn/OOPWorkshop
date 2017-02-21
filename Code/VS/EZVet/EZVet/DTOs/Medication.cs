using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EZVet.DTOs
{
    public class Medication : Entity<Medication, Domain.Treatment>
    {
        [MaxLength(30)]
        public virtual string Name { get; set; }
        public virtual string Dose { get; set; }
        public virtual DateTime Date { get; set; }

        public override Medication Initialize(Domain.Treatment domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            Date = domain.ContainingTreatment.Date;
            Dose = domain.Dose;

            return this;
        }
    }
}
