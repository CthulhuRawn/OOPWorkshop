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
            Map(x => x.Username);
            Map(x => x.BirthDate);
            Map(x => x.Email);

            References(x => x.Address);

            HasMany(x => x.Animals);
            Map(x => x.DoctorCode);
            HasMany(x => x.Treatments);
            HasMany(x => x.TreatingAnimals);
        }
    }
}
