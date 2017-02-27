using System;
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
        private readonly IDoctorsQueryProcessor _doctorsQueryProcessor;
        private readonly IDecodesQueryProcessor _decodesQueryProcessor;
        private readonly IAnimalsQueryProcessor _animalsQueryProcessor;

        public TreatmentsQueryProcessor(ISession session, IDoctorsQueryProcessor doctorsQueryProcessor, IDecodesQueryProcessor decodesQueryProcessor, IAnimalsQueryProcessor animalsQueryProcessor) : base(session)
        {
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
                var medications = treatment.Medications.Select(x =>
                    x.Id.HasValue && x.Id > 0
                        ? pastTreatments.Single(y => y.Id == x.Id)
                        : new Treatment
                        {
                            Name = x.Name,
                            Dose = x.Dose,
                            Price = x.Price,
                            Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Medication),
                            ContainingTreatment = report
                        });

                var treatments = treatment.Treatments.Select(x =>
                    x.Id.HasValue && x.Id > 0
                        ? pastTreatments.Single(y => y.Id == x.Id)
                        : new Treatment
                        {
                            Name = x.Name,
                            Price = x.Price,
                            Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Treatment),
                            ContainingTreatment = report
                        });

                var vaccines = treatment.Vaccines.Select(x =>
                    x.Id.HasValue && x.Id > 0
                        ? pastTreatments.Single(y => y.Id == x.Id)
                        : new Treatment
                        {
                            Name = x.Name,
                            Price = x.Price,
                            Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Vaccine),
                            ContainingTreatment = report
                        });

                var currentTreatments = new List<Treatment>();
                currentTreatments.AddRange(medications);
                currentTreatments.AddRange(vaccines);
                currentTreatments.AddRange(treatments);

                report.Treatments = currentTreatments;

                report.Date = DateTime.UtcNow;
                report.Summary = treatment.TreatmentSummary;

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

                treatments.AddRange(treatment.Medications.Select(x => new Treatment
                {
                    Name = x.Name,
                    Dose = x.Dose,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Medication),
                    ContainingTreatment = report
                }));

                treatments.AddRange(treatment.Vaccines.Select(x => new Treatment
                {
                    Name = x.Name,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Vaccine),
                    ContainingTreatment = report
                }));

                treatments.AddRange(treatment.Treatments.Select(x => new Treatment
                {
                    Name = x.Name,
                    Price = x.Price,
                    Type = _decodesQueryProcessor.Get<TreatmentTypeDecode>((int) TreatmentType.Treatment),
                    ContainingTreatment = report
                }));
                report.Summary = treatment.TreatmentSummary;
                report.AnimalMeasurements.ContainingTreatment = report;

                result = new TreatmentReport().Initialize(Save(report));
            }
            animal.Weight = result.Measurements.Weight;
            animal.DateNextVisit = treatment.Animal.NextVisit;
            _animalsQueryProcessor.Update(animal.Id, animal);

            return result;
        }
    }
}
