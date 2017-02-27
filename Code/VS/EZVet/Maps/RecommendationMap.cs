using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class RecommendationMap : ClassMap<Recommendation>
    {
        public RecommendationMap()
        {
            Id(x => x.Id);

            Map(x => x.Date);
            Map(x => x.Text);

            References(x => x.Owner);
        }
    }
}
