using Domain;

namespace Maps
{
    public class TreatmentMap : EntityMap<Treatment>
    {
        public TreatmentMap()
        {
            Map(x => x.Name);
            References(x => x.Type);
        }
    }
}

