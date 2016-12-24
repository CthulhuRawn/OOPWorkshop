using System;
using System.Collections.Generic;

namespace Domain
{
    public class TreatmentReport : Entity
    {
        public virtual Doctor Doctor { get; set; }
        public virtual Animal Animal { get; set; }
        public virtual AnimalMeasurements AnimalMeasurements { get; set; }
        public virtual IList<Treatment> Treatments { get; set; }
        public virtual double TotalPrice { get; set; }

    }
}
