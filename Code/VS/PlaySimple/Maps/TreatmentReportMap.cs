using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class TreatmentReportMap : EntityMap<TreatmentReport>
    {
        public TreatmentReportMap()
        {
            Map(x => x.TotalPrice);
            HasMany(x => x.Treatments);
            References(x => x.Animal);
            References(x => x.AnimalMeasurements);
            References(x => x.Doctor);
        }
    }
}
