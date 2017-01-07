using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class ComplaintMap : ClassMap<Complaint>
    {
        public ComplaintMap()
        {
            Id(x => x.Id);

            Map(x => x.Description);
            Map(x => x.Date);

            References(x => x.Type);
            References(x => x.OffendingCustomer);
            References(x => x.OffendedCustomer);
        }
    }
}
