﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using EZVet.Common;
using NHibernate;
using TreatmentReport = EZVet.DTOs.TreatmentReport;

namespace EZVet.QueryProcessors
{
    public interface ITreatmentsQueryProcessor
    {
        TreatmentReport Save(TreatmentReport treatment, int cotnactId);
    }


    public class TreatmentsQueryProcessor : DBAccessBase<Domain.TreatmentReport>, ITreatmentsQueryProcessor
    {
        private readonly IOwnersQueryProcessor _ownersQueryProcessor;
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;
        private readonly IAnimalsQueryProcessor _animalsQueryProcessor;

        public TreatmentsQueryProcessor(ISession session, IOwnersQueryProcessor ownersQueryProcessor, IDoctorsQueryProcessor doctorsQueryProcessor, IDecodesQueryProcessor decodesQueryProcessor, IAnimalsQueryProcessor animalsQueryProcessor) : base(session)
        {
            _ownersQueryProcessor = ownersQueryProcessor;
            _doctorsQueryProcessor = doctorsQueryProcessor;
            _decodesQueryProcessor = decodesQueryProcessor;
            _animalsQueryProcessor = animalsQueryProcessor;
        }

        public TreatmentReport Save(TreatmentReport treatment, int cotnactId)
        {
            TreatmentReport result;
            var animal = _animalsQueryProcessor.Get(treatment.Animal.Id.Value);
            if (treatment.Id.HasValue)
            {
                var report = Get(treatment.Id.Value);

                if (treatment.Measurements != null)
                {
                    report.AnimalMeasurements = report.AnimalMeasurements ?? new AnimalMeasurements();

                    report.AnimalMeasurements.DiastolicBloodPressure = treatment.Measurements.DiastolicBloodPressure;
                    report.AnimalMeasurements.SystolicBloodPressure = treatment.Measurements.SystolicBloodPressure;
                    report.AnimalMeasurements.Pulse = treatment.Measurements.Pulse;
                    report.AnimalMeasurements.Temperature = treatment.Measurements.Temperature;
                    report.AnimalMeasurements.Weight = treatment.Measurements.Weight;
                }

                var pastTreatments = report.Treatments.ToList();
                var medicationToAdd = treatment.Medications.Where(x => pastTreatments.All(y => y.Id != x.Id)).Select(x => new Treatment
                {
                    Name = x.Name,
                    Dose = x.Dose,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int)TreatmentType.Medication)
                });

                var treatmentsToAdd = treatment.Treatments.Where(x => pastTreatments.All(y => y.Id != x.Id)).Select(x => new Treatment
                {
                    Name = x.Name,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int)TreatmentType.Treatment)
                });

                var vaccinesToAdd = treatment.Vaccines.Where(x => pastTreatments.All(y => y.Id != x.Id)).Select(x => new Treatment
                {
                    Name = x.Name,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int)TreatmentType.Vaccine)
                });

                var treatments = new List<Treatment>();
                treatments.AddRange(medicationToAdd);
                treatments.AddRange(vaccinesToAdd);
                treatments.AddRange(treatmentsToAdd);

                treatments.AddRange(report.Treatments);

                report.Treatments = report.Treatments.Where(x => treatment.Vaccines.Any(y => y.Id == x.Id))
                    .Where(x => treatment.Medications.Any(y => y.Id == x.Id))
                    .Where(x => treatment.Treatments.Any(y => y.Id == x.Id)).ToList();

                report.Treatments = treatments;

                report.Date = DateTime.UtcNow;

                Update(report.Id, report);
                result = new TreatmentReport().Initialize(report);
            }
            else
            {
                var animalMeasurements = new AnimalMeasurements
                {
                    DiastolicBloodPressure = treatment.Measurements.DiastolicBloodPressure,
                    SystolicBloodPressure = treatment.Measurements.SystolicBloodPressure,
                    Pulse = treatment.Measurements.Pulse,
                    Temperature = treatment.Measurements.Temperature,
                    Weight = treatment.Measurements.Weight,
                };
                animal.AnimalMeasurements.Add(animalMeasurements);

                var treatments = new List<Treatment>();

                treatments.AddRange(treatment.Medications.Select(x => new Treatment
                {
                    Name = x.Name,
                    Dose = x.Dose,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Medication)
                }));

                treatments.AddRange(treatment.Vaccines.Select(x => new Treatment
                {
                    Name = x.Name,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Vaccine)
                }));

                treatments.AddRange(treatment.Treatments.Select(x => new Treatment
                {
                    Name = x.Name,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Treatment)
                }));

                var report = new Domain.TreatmentReport
                {
                    AnimalMeasurements = animalMeasurements,
                    Animal = _animalsQueryProcessor.Get(treatment.Animal.Id.Value),
                    Date = DateTime.UtcNow,
                    TotalPrice =
                        treatment.Medications.Sum(x => x.Price) + treatment.Vaccines.Sum(x => x.Price) +
                        treatment.Medications.Sum(x => x.Price),
                    Treatments = treatments,
                    Doctor = _doctorsQueryProcessor.Get(cotnactId)
                };
                
                result = new TreatmentReport().Initialize(Save(report));
            }
            animal.Weight = result.Measurements.Weight;
            animal.DateNextVisit = treatment.Animal.NextVisit;
            _animalsQueryProcessor.Update(animal.Id, animal);

            return result;
        }
    }
}
