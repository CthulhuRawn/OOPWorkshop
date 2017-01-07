using System;
using System.Collections.Generic;

namespace Domain
{
    public class Customer : Owner
    {
        public virtual DateTime? FreezeDate { get; set; }

        public virtual RegionDecode Region { get; set; }

        public virtual IList<Order> Orders { get; set; }

        public virtual IList<Participant> ParticipationRequests {get; set;}

        public virtual IList<Complaint> Complaints { get; set; }

        public virtual IList<Review> Reviews { get; set; }
    }
}
