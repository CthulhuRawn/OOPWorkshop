using System;
using System.Collections.Generic;
using System.Linq;
using DTO.Validators;

namespace DTO
{
    public class TreatmentReport : Entity<TreatmentReport, Domain.TreatmentReport>
    {
        [ExistsInDB(typeof(Domain.Animal))]
        public Animal Animal{ get; set; }
        public IList<Treatment> Treatments { get; set; }
        public IList<Medication> Medications { get; set; }
        public IList<Vaccine> Vaccines { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public AnimalMeasurements Measurements { get; set; }
        public string TreatmentSummary { get; set; }
        public string DoctorName { get; set; }
        public string DoctorPhone { get; set; }

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
            Treatments = domain.Treatments.Where(x => x.Type.Id == (int)Consts.Decodes.TreatmentType.Treatment).Select(x => new Treatment().Initialize(x)).ToList();
            Vaccines = domain.Treatments.Where(x => x.Type.Id == (int)Consts.Decodes.TreatmentType.Vaccine).Select(x => new Vaccine().Initialize(x)).ToList();
            Medications = domain.Treatments.Where(x => x.Type.Id == (int)Consts.Decodes.TreatmentType.Medication).Select(x => new Medication().Initialize(x)).ToList();
            Measurements = new AnimalMeasurements().Initialize(domain.AnimalMeasurements);
            TreatmentSummary = domain.Summary;
            TotalPrice = domain.Treatments.Select(x => x.Price).Sum();
            DoctorName = domain.Doctor.GetName();
            DoctorPhone = domain.Doctor.Phone;
        }
    }
}
