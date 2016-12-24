using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class FieldSizeDecodeMap : ClassMap<FieldSizeDecode>
    {
        public FieldSizeDecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
