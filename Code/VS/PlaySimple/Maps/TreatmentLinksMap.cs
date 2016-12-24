using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class TreatmentLinksMap : EntityMap<TreatmentLinks>
    {
        public virtual Doctor Doctor { get; set; }
        public virtual Treatment Treatment { get; set; }
        public virtual double Price { get; set; }

        public TreatmentLinksMap()
        {
            Map(x => x.Price);
            References(x => x.Treatment);
            References(x => x.Doctor);
        }
    }
}
