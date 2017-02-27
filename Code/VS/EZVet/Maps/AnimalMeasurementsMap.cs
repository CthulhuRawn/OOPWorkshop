using Domain;

namespace Maps
{
    public class AnimalMeasurementsMap : EntityMap<AnimalMeasurements>
    {
        public AnimalMeasurementsMap()
        {
            Map(x => x.Pulse);
            Map(x => x.Temperature);
            Map(x => x.Weight);
            Map(x => x.SystolicBloodPressure);
            Map(x => x.DiastolicBloodPressure);
            References(x => x.ContainingTreatment);
        }
    }
}
