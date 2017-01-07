using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class AdminMap : ClassMap<Admin>
    {
        public AdminMap()
        {
            Id(x => x.Id);

            Map(x => x.Username);
            Map(x => x.Password);
        }
    }
}
