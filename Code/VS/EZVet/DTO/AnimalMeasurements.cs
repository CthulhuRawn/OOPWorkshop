using System;
using DTO.Validators;

namespace DTO
{
    public class AnimalMeasurements : Entity<AnimalMeasurements, Domain.AnimalMeasurements>
    {
        [Above(0)]
        public virtual int Pulse { get; set; }
        [Above(0)]
        public virtual double Temperature { get; set; }
        [Above(0)]
        public virtual double Weight { get; set; }
        [Above(0)]
        public virtual int SystolicBloodPressure { get; set; }
        [Above(0)]
        public virtual int DiastolicBloodPressure { get; set; }

        public virtual DateTime? Date { get; set; }

        public override AnimalMeasurements Initialize(Domain.AnimalMeasurements domain)
        {
            Id = domain.Id;
            Pulse = domain.Pulse;
            Weight = domain.Weight;
            Temperature = domain.Temperature;
            SystolicBloodPressure = domain.SystolicBloodPressure;
            DiastolicBloodPressure = domain.DiastolicBloodPressure;
            Date = domain.ContainingTreatment?.Date;
            
            return this;
        }
    }
}
