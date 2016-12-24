using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class DoctorMap : UserBaseMap<Doctor>
    {
        public DoctorMap()
        {
            Map(x => x.DoctorCode);
            HasMany(x => x.Treatments);
            HasMany(x => x.TreatingAnimals);
        }
    }
}
