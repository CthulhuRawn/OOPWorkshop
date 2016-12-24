using System;
using System.Collections.Generic;

namespace Domain
{
    public class Order : Entity
    {
        public virtual DateTime StartDate { get; set; }

        public virtual Customer Owner { get; set; }

        public virtual int PlayersNumber { get; set; }

        public virtual OrderStatusDecode Status { get; set; }

        public virtual Field Field { get; set; }

        public virtual IList<Participant> Participants { get; set; }
    }
}
