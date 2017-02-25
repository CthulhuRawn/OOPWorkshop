using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Domain;

namespace EZVet.DTOs
{
    public class TreatmentReport : Entity<TreatmentReport, Domain.TreatmentReport>
    {
        public virtual Animal Animal{ get; set; }
        public virtual IList<Treatment> Treatments { get; set; }
        public virtual IList<Medication> Medications { get; set; }
        public virtual IList<Vaccine> Vaccines { get; set; }
        public virtual double TotalPrice { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual AnimalMeasurements Measurements { get; set; }

        public override TreatmentReport Initialize(Domain.TreatmentReport domain)
        {
            Id = domain.Id;
            Date = domain.Date;
            Treatments = domain.Treatments.Where(x => x.Type.Name == "Treatment").Select(x=>new Treatment().Initialize(x)).ToList();
            Vaccines = domain.Treatments.Where(x => x.Type.Name == "Vaccine").Select(x=>new Vaccine().Initialize(x)).ToList();
            Medications = domain.Treatments.Where(x => x.Type.Name == "Medication").Select(x=>new Medication().Initialize(x)).ToList();
            Measurements = new AnimalMeasurements().Initialize(domain.AnimalMeasurements);
            Animal = new Animal().Initialize(domain.Animal);

            return this;
        }
    }
}
