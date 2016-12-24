using System;
using System.Collections.Generic;

namespace Domain
{
    public class TreatmentLinks : Entity
    {
        public virtual Doctor Doctor { get; set; }
        public virtual Treatment Treatment { get; set; }
        public virtual double Price { get; set; }
    }
}
