using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class OrderStatusDecodeMap : ClassMap<OrderStatusDecode>
    {
        public OrderStatusDecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
