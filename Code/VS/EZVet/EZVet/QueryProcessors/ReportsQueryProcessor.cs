using System;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;
using EZVet.DTOs;

namespace EZVet.QueryProcessors
{
    public interface IReportsQueryProcessor
    {
       IEnumerable<FinanceReport> GetFinanceReport(DateTime? startDate, DateTime? endDate, int? datePart, int doctorId, int ownerId);
    }

    public class ReportsQueryProcessor : IReportsQueryProcessor
    {
        private ITreatmentsQueryProcessor _treatmentsQueryProcessor;
        private Dictionary<int, string> datePartFormat;
        public ReportsQueryProcessor(ITreatmentsQueryProcessor treatmentsQueryProcessor)
        {
            _treatmentsQueryProcessor = treatmentsQueryProcessor;
            datePartFormat = new Dictionary<int, string>
            {
                {1, "dd/MM/yyyy"},
                {2, "MM/yyyy"},
                {3, "yyyy"}
            };
        }
        

        public IEnumerable<FinanceReport> GetFinanceReport(DateTime? startDate, DateTime? endDate, int? datePart, int doctorId, int ownerId)
        {
            startDate = startDate ?? new DateTime(1900, 1, 1);
            endDate = endDate ?? DateTime.MaxValue;
            datePart = datePart ?? 1;
            return _treatmentsQueryProcessor.Query()
                .Where(
                    x =>
                        (x.Doctor.Id == doctorId || doctorId == -1) ||
                        (x.Animal.Owner.Id == ownerId || ownerId == -1) && x.Date >= startDate && x.Date <= endDate)
                .ToList()
                .GroupBy(x => x.Date.ToString(datePartFormat[datePart.Value])).Select(x => new FinanceReport
                {
                    Date = x.Key,
                    NumOfVisits = x.Count(),
                    Price = x.Sum(y => y.TotalPrice)
                });
        }
    }
}