using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class AddressMap : EntityMap<Address>
    {
        public AddressMap()
        {
            Map(x => x.Country);
            Map(x => x.City);
            Map(x => x.StreetName);
            Map(x => x.StreetNumber);
        }
    }
}
