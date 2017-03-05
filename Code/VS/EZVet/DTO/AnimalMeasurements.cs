using System;
using DTO.Validators;

namespace DTO
{
    public class AnimalMeasurements : Entity<AnimalMeasurements, Domain.AnimalMeasurements>
    {
        [Above(0)]
        public int Pulse { get; set; }
        [Above(0)]
        public double Temperature { get; set; }
        [Above(0)]
        public double Weight { get; set; }
        [Above(0)]
        public int SystolicBloodPressure { get; set; }
        [Above(0)]
        public int DiastolicBloodPressure { get; set; }

        public DateTime? Date { get; set; }

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
