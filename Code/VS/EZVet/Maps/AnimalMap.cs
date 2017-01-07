using Domain;

namespace Maps
{
    public class AnimalMap : EntityMap<Animal>
    {
        public AnimalMap()
        {
            Map(x => x.Name);
            Map(x => x.Weight);
            Map(x => x.Notes);
            Map(x => x.DateOfBirth);

            References(x => x.Gender);
            References(x => x.Owner);
            References(x => x.Type);
            References(x => x.SubType);

            HasMany(x => x.Orders);
            HasMany(x => x.Doctors);
            HasMany(x => x.AnimalMeasurements);
        }
    }
}
