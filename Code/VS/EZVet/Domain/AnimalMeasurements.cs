namespace Domain
{
    public class AnimalMeasurements : Entity
    {
        public virtual int Pulse { get; set; }
        public virtual double Temprature { get; set; }
        public virtual double Weight { get; set; }
        public virtual int SystolicBloodPressure { get; set; }
        public virtual int DiastolicBloodPressure { get; set; }
    }
}
