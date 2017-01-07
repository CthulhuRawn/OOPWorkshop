using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Id(x => x.Id);

            Map(x => x.StartDate);
            Map(x => x.PlayersNumber);

            References(x => x.Field);
            References(x => x.Status);
            References(x => x.Owner);

            HasMany(x => x.Participants);
        }
    }
}
