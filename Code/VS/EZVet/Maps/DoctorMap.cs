using Domain;

namespace Maps
{
    public class DoctorMap : EntityMap<Doctor>
    {
        public DoctorMap()
        {
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Password);
            Map(x => x.BirthDate);
            Map(x => x.Email);
            Map(x => x.DoctorCode);

            References(x => x.Address).Cascade.All();

            HasMany(x => x.Animals);
            HasMany(x => x.Treatments);
            HasMany(x => x.TreatingAnimals);
        }
    }
}
