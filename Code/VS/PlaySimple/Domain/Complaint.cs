using System;

namespace Domain
{
    public class Complaint : Entity
    {
        public virtual string Description { get; set; }

        public virtual ComplaintTypeDecode Type { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual Customer OffendingCustomer { get; set; }

        public virtual Customer OffendedCustomer { get; set; }
    }
}
