using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class AnimalMeasurementsMap : EntityMap<AnimalMeasurements>
    {
        public AnimalMeasurementsMap()
        {
            Map(x => x.Pulse);
            Map(x => x.Temprature);
            Map(x => x.Weight);
            Map(x => x.SystolicBloodPressure);
            Map(x => x.DiastolicBloodPressure);
        }
    }
}
