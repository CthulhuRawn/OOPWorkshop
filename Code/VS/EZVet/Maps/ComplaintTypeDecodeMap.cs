using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    class ComplaintTypeDecodeMap : ClassMap<ComplaintTypeDecode>
    {
        public ComplaintTypeDecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
