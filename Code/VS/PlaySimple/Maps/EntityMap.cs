using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public abstract class EntityMap<TEntity> : ClassMap<TEntity>
		where TEntity : Entity
    {
        public EntityMap()
        {
            Id(x => x.Id);
        }
    }
}
