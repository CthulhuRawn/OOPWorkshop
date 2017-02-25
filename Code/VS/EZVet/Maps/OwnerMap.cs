using Domain;

namespace Maps
{
    public class OwnerMap : EntityMap<Owner>
    {
        public OwnerMap()
        {
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Password);
            Map(x => x.BirthDate);
            Map(x => x.Email);
            Map(x => x.Phone);

            References(x => x.Address).Cascade.All();

            HasMany(x => x.Animals);
        }
    }
}
