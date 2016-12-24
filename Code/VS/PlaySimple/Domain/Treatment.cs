using System;
using System.Collections.Generic;

namespace Domain
{
    public class Treatment : Entity
    {
        public virtual string Name { get; set; }
        public virtual TreatmentTypeDecode Type { get; set; }
    }
}
