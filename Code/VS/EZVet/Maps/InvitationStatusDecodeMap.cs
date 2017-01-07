using Domain;
using FluentNHibernate.Mapping;

namespace Maps
{
    public class InvitationStatusDecodeMap : ClassMap<InvitationStatusDecode>
    {
        public InvitationStatusDecodeMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
