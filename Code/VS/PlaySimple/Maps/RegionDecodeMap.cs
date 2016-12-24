using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class RegionDecodeMap : ClassMap<RegionDecode>
    {
        public RegionDecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
