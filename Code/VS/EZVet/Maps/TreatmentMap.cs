using Domain;

namespace Maps
{
    public class TreatmentMap : EntityMap<Treatment>
    {
        public TreatmentMap()
        {
            Map(x => x.Name);
            Map(x => x.Dose);
            References(x => x.Type);
            References(x => x.ContainingTreatment);
        }
    }
}

