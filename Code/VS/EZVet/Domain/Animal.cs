using System;
using System.Collections.Generic;

namespace Domain
{
    public class Animal : Entity
    {
        public virtual string Name { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual IList<TreatmentReport> Orders { get; set; }
        public virtual IList<Doctor> Doctors { get; set; }
        public virtual IList<AnimalMeasurements> AnimalMeasurements { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual GenderDecode Gender { get; set; }
        public virtual double Weight { get; set; }
        public virtual AnimalTypeDecode Type { get; set; }
        public virtual AnimalSubTypeDecode SubType { get; set; }
        public virtual string Notes { get; set; }
        public virtual string ChipNumber { get; set; }
        public virtual string Color { get; set; }
    }
}
