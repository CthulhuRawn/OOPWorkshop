﻿using System;
using System.Collections.Generic;
using System.Linq;

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
        public DateTime NextVisit { get; set; }
        public string Notes { get; set; }
        public double Weight { get; set; }
        public string ChipNumber { get; set; }
        public string Color { get; set; }
        public IEnumerable<Treatment> Treatments { get; set; }
        public IEnumerable<Vaccine> Vaccines { get; set; }
        public IEnumerable<Medication> Medications { get; set; }


        public override Animal Initialize(Domain.Animal domain)
        {
            var previousOrder = domain.Orders?.OrderByDescending(x => x.Id).FirstOrDefault();
            Id = domain.Id;
            Name = domain.Name;
            DoctorName = previousOrder?.Doctor.FirstName + previousOrder?.Doctor.LastName;
            Type = domain.Type.Id;
            DateOfBirth = domain.DateOfBirth;
            Gender = domain.Gender.Id;
            NextVisit = DateTime.UtcNow.AddDays(180);
            OwnerName = domain.Owner.FirstName + domain.Owner.LastName;
            Notes = domain.Notes;
            Weight = domain.Weight;
            ChipNumber = domain.ChipNumber;
            Color = domain.Color;

            return this;
        }
    }
}
