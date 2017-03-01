using System;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Animal : Entity<Animal, Domain.Animal>
    {
        public string Name { get; set; }
        public string DoctorName { get; set; }
        public string OwnerName { get; set; }
        public int Type { get; set; }

        [NotInFuture]
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }

        [NotInPast]
        public DateTime? NextVisit { get; set; }
        public string Notes { get; set; }
        public double Weight { get; set; }
        public string ChipNumber { get; set; }
        public string Color { get; set; }
        public string OwnerPhone { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
        public IEnumerable<Vaccine> Vaccines { get; set; }
        public IEnumerable<Medication> Medications { get; set; }
        public IEnumerable<TreatmentReport> TreatmentSummaries { get; set; }
        public IEnumerable<AnimalMeasurements> AnimalMeasurements { get; set; }


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
            TreatmentSummaries = domain.Orders?.Select(x => new TreatmentReport().ShallowInitialize(x));
            AnimalMeasurements = domain.AnimalMeasurements?.Select(x=>new AnimalMeasurements().Initialize(x));

            return this;
        }
    }
}
