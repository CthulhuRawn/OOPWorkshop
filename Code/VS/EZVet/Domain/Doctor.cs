using System.Collections.Generic;

namespace Domain
{
    public class Doctor : Owner
    {
        public virtual IList<TreatmentLinks> Treatments { get; set; }
        public virtual string DoctorCode { get; set; }
        public virtual IList<Animal> TreatingAnimals { get; set; }
        public virtual string Notes { get; set; }
        public virtual string OpeningHours { get; set; }
        public virtual string Phone { get; set; }
        public virtual IList<AnimalTypeDecode> AnimalTypes { get; set; }
    }
}
