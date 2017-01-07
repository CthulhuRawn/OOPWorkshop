using System;

namespace Domain
{
    public class Review : Entity
    {
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual Customer Reviewer { get; set; }

        public virtual Customer ReviewedCustomer { get; set; }
    }
}
