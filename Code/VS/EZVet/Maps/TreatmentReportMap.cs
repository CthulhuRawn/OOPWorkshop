using Domain;

namespace Maps
{
    public class TreatmentReportMap : EntityMap<TreatmentReport>
    {
        public TreatmentReportMap()
        {
            Map(x => x.TotalPrice);
            Map(x => x.Date);
            Map(x => x.Summary);
            HasMany(x => x.Treatments).Cascade.All();
            References(x => x.Animal);
            References(x => x.AnimalMeasurements).Cascade.All();
            References(x => x.Doctor);
        }
    }
}
