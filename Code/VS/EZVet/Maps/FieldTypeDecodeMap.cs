using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class FieldTypeDecodeMap : ClassMap<FieldTypeDecode>
    {
        public FieldTypeDecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
