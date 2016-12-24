using System;

namespace Domain
{
    public class Participant : Entity
    {
        public virtual Customer Customer { get; set; }

        public virtual Order Order { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual InvitationStatusDecode Status { get; set; }
    }
}
