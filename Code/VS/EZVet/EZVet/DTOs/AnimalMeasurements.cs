namespace EZVet.DTOs
{
    public class AnimalMeasurements : Entity<AnimalMeasurements, Domain.AnimalMeasurements>
    {
        public virtual int Pulse { get; set; }
        public virtual double Temperature { get; set; }
        public virtual double Weight { get; set; }
        public virtual int SystolicBloodPressure { get; set; }
        public virtual int DiastolicBloodPressure { get; set; }

        public override AnimalMeasurements Initialize(Domain.AnimalMeasurements domain)
        {
            Id = domain.Id;
            Pulse = domain.Pulse;
            Weight = domain.Weight;
            Temperature = domain.Temperature;
            SystolicBloodPressure = domain.SystolicBloodPressure;
            DiastolicBloodPressure = domain.DiastolicBloodPressure;
            
            return this;
        }
    }
}
