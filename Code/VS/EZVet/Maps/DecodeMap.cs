using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public abstract class DecodeMap<TEntity> : ClassMap<TEntity>
		where TEntity : Decode
    {
        public DecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
