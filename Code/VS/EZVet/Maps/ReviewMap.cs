using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class ReviewMap : ClassMap<Review>
    {
        public ReviewMap()
        {
            Id(x => x.Id);

            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.Date);

            References(x => x.Reviewer);
            References(x => x.ReviewedCustomer);
        }
    }
}
