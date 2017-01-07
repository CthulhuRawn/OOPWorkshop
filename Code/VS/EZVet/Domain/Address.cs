using System;
using System.Collections.Generic;

namespace Domain
{
    public class Address : Entity
    {
        public virtual string Country { get; set; }
        public virtual string City { get; set; }
        public virtual string StreetName { get; set; }
        public virtual int StreetNumber { get; set; }
    }
}
