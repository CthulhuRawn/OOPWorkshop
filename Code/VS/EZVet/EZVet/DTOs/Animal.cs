using System;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;

namespace EZVet.DTOs
{
    public class Animal : Entity<Animal, Domain.Animal>
    {
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public string OwnerName { get; set; }
        public int Type { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public DateTime? NextVisit { get; set; }
        public string Notes { get; set; }
        public double Weight { get; set; }
        public string ChipNumber { get; set; }
        public string Color { get; set; }
        public string OwnerPhone { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
        public IEnumerable<Vaccine> Vaccines { get; set; }
        public IEnumerable<Medication> Medications { get; set; }


        public override Animal Initialize(Domain.Animal domain)
        {
            Id = domain.Id;
            Name = domain.Name;
            DoctorName = domain.Doctor?.GetName();
            Type = domain.Type.Id;
            DateOfBirth = domain.DateOfBirth;
            Gender = domain.Gender.Id;
            NextVisit = domain.DateNextVisit;
            OwnerName = domain.Owner.GetName();
            Notes = domain.Notes;
            Weight = domain.Weight;
            ChipNumber = domain.ChipNumber;
            Color = domain.Color;
            OwnerPhone = domain.Owner.Phone;
            Vaccines =
                domain.Orders?.SelectMany(x => x.Treatments)
                    .Where(x => x.Type.Id == (int) TreatmentType.Vaccine)
                    .Select(x => new Vaccine().Initialize(x))
                    .ToList();
            Medications =
                domain.Orders?.SelectMany(x => x.Treatments)
                    .Where(x => x.Type.Id == (int) TreatmentType.Medication)
                    .Select(x => new Medication().Initialize(x))
                    .ToList();
            Treatments =
                domain.Orders?.SelectMany(x => x.Treatments)
                    .Where(x => x.Type.Id == (int) TreatmentType.Treatment)
                    .Select(x => new Treatment().Initialize(x))
                    .ToList();


            return this;
        }
    }
}
