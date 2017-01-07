using System.Collections.Generic;

namespace Domain
{
    public class Doctor : Owner
    {
        public virtual IList<TreatmentLinks> Treatments { get; set; }
        public virtual string DoctorCode { get; set; }
        public virtual IList<Animal> TreatingAnimals { get; set; }
    }
}
