﻿using System.Collections.Generic;

namespace Domain
{
    public class Doctor : Person
    {
        public virtual string DoctorCode { get; set; }
        public virtual IList<Animal> TreatingAnimals { get; set; }
        public virtual string Notes { get; set; }
        public virtual string OpeningHours { get; set; }
        public virtual IList<Recommendation> Recommendations { get; set; }
        public virtual IList<AnimalTypeDecode> AnimalTypes { get; set; }
    }
}
