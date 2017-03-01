using System;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;
using EZVet.DTOs;

namespace EZVet.QueryProcessors
{
    public interface IReportsQueryProcessor
    {
        IEnumerable<FinanceReport> GetFinanceReport(DateTime? startDate, DateTime? endDate, int? datePart, int doctorId,
            int ownerId);

        IEnumerable<ItemUsageReport> GetPerItemReport(DateTime? startDate, DateTime? endDate, int? datePart,
            int doctorId, string itemName);

        IEnumerable<AnimalTypeReport> GetPerTypeReport(DateTime? startDate, DateTime? endDate, int? datePart,
            int doctorId, int? animalType);
        IEnumerable<AnimalNameReport> GetPerAnimalReport(DateTime? startDate, DateTime? endDate, int? datePart,
            int ownerId, int? animalId);

        IEnumerable<VisitsReport> GetVisitsReport(int? time, int doctorId, int ownerId);
    }

    public class ReportsQueryProcessor : IReportsQueryProcessor
    {
        private ITreatmentsQueryProcessor _treatmentsQueryProcessor;
        private IAnimalsQueryProcessor _animalsQueryProcessor;
        private readonly Dictionary<int, string> _datePartFormat;

        public ReportsQueryProcessor(ITreatmentsQueryProcessor treatmentsQueryProcessor, IAnimalsQueryProcessor animalsQueryProcessor)
        {
            _treatmentsQueryProcessor = treatmentsQueryProcessor;
            _animalsQueryProcessor = animalsQueryProcessor;
            _datePartFormat = new Dictionary<int, string>
            {
                {1, "dd/MM/yyyy"},
                {2, "MM/yyyy"},
                {3, "yyyy"}
            };
        }
        
        public IEnumerable<FinanceReport> GetFinanceReport(DateTime? startDate, DateTime? endDate, int? datePart,
            int doctorId, int ownerId)
        {
            startDate = startDate ?? new DateTime(1900, 1, 1);
            endDate = endDate ?? DateTime.MaxValue;
            datePart = datePart ?? 1;
            return _treatmentsQueryProcessor.Query()
                .Where(
                    x =>
                        (x.Doctor.Id == doctorId || x.Animal.Owner.Id == ownerId) 
                        && x.Date >= startDate && x.Date <= endDate)
                .ToList()
                .GroupBy(x => x.Date.ToString(_datePartFormat[datePart.Value])).Select(x => new FinanceReport
                {
                    Date = x.Key,
                    Count = x.Count(),
                    Price = x.Sum(y => y.TotalPrice)
                });
        }

        public IEnumerable<ItemUsageReport> GetPerItemReport(DateTime? startDate, DateTime? endDate, int? datePart,
            int doctorId, string itemName)
        {
            startDate = startDate ?? new DateTime(1900, 1, 1);
            endDate = endDate ?? DateTime.MaxValue;
            datePart = datePart ?? 1;
            return _treatmentsQueryProcessor.Query()
                .Where(
                    x =>
                        x.Doctor.Id == doctorId && x.Date >= startDate && x.Date <= endDate)
                .SelectMany(x => x.Treatments).Where(x => x.Name.Contains(itemName))
                .ToList()
                .GroupBy(x => new {Date = x.ContainingTreatment.Date.ToString(_datePartFormat[datePart.Value]), x.Name})
                .Select(x => new ItemUsageReport
                {
                    Date = x.Key.Date,
                    ItemName = x.Key.Name,
                    Count = x.Count(),
                    Price = x.Sum(y => y.Price)
                });
        }

        public IEnumerable<AnimalTypeReport> GetPerTypeReport(DateTime? startDate, DateTime? endDate, int? datePart, int doctorId, int? animalType)
        {
            startDate = startDate ?? new DateTime(1900, 1, 1);
            endDate = endDate ?? DateTime.MaxValue;
            datePart = datePart ?? 1;
            animalType = animalType ?? -1;
            return _treatmentsQueryProcessor.Query()
                .Where(
                    x =>
                        x.Doctor.Id == doctorId && x.Date >= startDate && x.Date <= endDate && (x.Animal.Type.Id == animalType || animalType == -1))
                .ToList()
                .GroupBy(x => new { Date = x.Date.ToString(_datePartFormat[datePart.Value]), x.Animal.Type.Name })
                .Select(x => new AnimalTypeReport
                {
                    Date = x.Key.Date,
                    AnimalType= x.Key.Name,
                    Count = x.Count(),
                    Price = x.Sum(y => y.TotalPrice)
                });
        }

        public IEnumerable<AnimalNameReport> GetPerAnimalReport(DateTime? startDate, DateTime? endDate, int? datePart, int ownerId, int? animalId)
        {
            startDate = startDate ?? new DateTime(1900, 1, 1);
            endDate = endDate ?? DateTime.MaxValue;
            datePart = datePart ?? 1;
            animalId = animalId ?? -1;
            return _treatmentsQueryProcessor.Query()
                .Where(
                    x =>
                        x.Animal.Owner.Id == ownerId && x.Date >= startDate && x.Date <= endDate && (x.Animal.Id == animalId || animalId == -1))
                .ToList()
                .GroupBy(x => new { Date = x.Date.ToString(_datePartFormat[datePart.Value]), x.Animal.Name })
                .Select(x => new AnimalNameReport()
                {
                    Date = x.Key.Date,
                    AnimalName = x.Key.Name,
                    Count = x.Count(),
                    Price = x.Sum(y => y.TotalPrice)
                });
        }

        public IEnumerable<VisitsReport> GetVisitsReport(int? time, int doctorId, int ownerId)
        {
            var isDoctorMode = doctorId < ownerId;
            time = time ?? 1;
            return _animalsQueryProcessor.Query()
                .Where(
                    x =>
                        x.Doctor.Id == doctorId ||
                        x.Owner.Id == ownerId && time == 1 && x.DateNextVisit < DateTime.UtcNow ||
                        time == 2 && x.DateNextVisit > DateTime.UtcNow).Select(x => new VisitsReport
                {
                    Date = x.DateNextVisit.Value,
                    AnimalName = x.Name,
                    EntityName = isDoctorMode ? x.Doctor.GetName() : x.Owner.GetName(),
                    EntityPhone = isDoctorMode ? x.Doctor.Phone : x.Owner.Phone
                }).ToList();
        }
    }
}
