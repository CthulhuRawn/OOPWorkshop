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
            Map(x => x.ChipNumber);
            Map(x => x.Color);

            References(x => x.Gender);
            References(x => x.Owner);
            References(x => x.Type);
            References(x => x.SubType);
            References(x => x.Doctor);

            HasMany(x => x.Orders);
            HasMany(x => x.AnimalMeasurements);
        }
    }
}
