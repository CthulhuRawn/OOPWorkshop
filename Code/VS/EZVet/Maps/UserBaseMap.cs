using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public abstract class UserBaseMap<TEntity> : EntityMap<TEntity>
        where TEntity : UserBase
    {
        public UserBaseMap()
        {
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Password);
            Map(x => x.Username);
            Map(x => x.BirthDate);
            Map(x => x.Email);

            References(x => x.Address);

            HasMany(x => x.Animals);
        }
    }
}
