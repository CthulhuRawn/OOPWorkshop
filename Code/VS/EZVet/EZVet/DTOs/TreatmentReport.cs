using System;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;

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
        public virtual string TreatmentSummary { get; set; }

        public override TreatmentReport Initialize(Domain.TreatmentReport domain)
        {
            CommonInitialize(domain);
            Animal = new Animal().Initialize(domain.Animal);
         
            return this;
        }

        public override TreatmentReport ShallowInitialize(Domain.TreatmentReport domain)
        {
            CommonInitialize(domain);
            return this;
        }

        private void CommonInitialize(Domain.TreatmentReport domain)
        {
            Id = domain.Id;
            Date = domain.Date;
            Treatments = domain.Treatments.Where(x => x.Type.Id == (int)TreatmentType.Treatment).Select(x => new Treatment().Initialize(x)).ToList();
            Vaccines = domain.Treatments.Where(x => x.Type.Id == (int)TreatmentType.Vaccine).Select(x => new Vaccine().Initialize(x)).ToList();
            Medications = domain.Treatments.Where(x => x.Type.Id == (int)TreatmentType.Medication).Select(x => new Medication().Initialize(x)).ToList();
            Measurements = new AnimalMeasurements().Initialize(domain.AnimalMeasurements);
            TreatmentSummary = domain.Summary;
            TotalPrice = domain.Treatments.Select(x => x.Price).Sum();
        }
    }
}
