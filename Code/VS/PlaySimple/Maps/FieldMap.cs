using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class FieldMap : ClassMap<Field>
    {
        public FieldMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);

            References(x => x.Type);
            References(x => x.Size);
        }
    }
}
