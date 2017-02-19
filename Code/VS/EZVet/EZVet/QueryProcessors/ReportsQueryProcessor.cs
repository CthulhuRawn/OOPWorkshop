using System;
using System.Collections.Generic;
using System.Linq;
using EZVet.Common;
using EZVet.DTOs;

namespace EZVet.QueryProcessors
{
    public interface IReportsQueryProcessor
    {
        IEnumerable<OffendingCustomersReport> GetOffendingCustomersReport(DateTime? fromDate, DateTime? untilDate, int? complaintType);
        IEnumerable<CustomersActivityReport> GetCustomersActivityReport(string firstName, string lastName, int? minAge, int? maxAge, DateTime? fromDate, DateTime? untilDate);
        IEnumerable<UsingFieldsReport> GetUsingFieldsReport(int? fieldId, string fieldName, DateTime? fromDate, DateTime? untilDate);
    }

    public class ReportsQueryProcessor : IReportsQueryProcessor
    {
        private readonly IOwnersQueryProcessor _customersQueryProcessor;
        private readonly IComplaintsQueryProcessor _complaintsQueryProcessor;
        private readonly IOrdersQueryProcessor _ordersQueryProcessor;
        private readonly IParticipantsQueryProcessor _participantsQueryProcessor;
        private readonly IFieldsQueryProcessor _fieldsQueryProcessor;

        public ReportsQueryProcessor(IOwnersQueryProcessor customersQueryProcessor, IOrdersQueryProcessor ordersQueryProcessor, IComplaintsQueryProcessor complaintsQueryProcessor, IParticipantsQueryProcessor participantsQueryProcessor, IFieldsQueryProcessor fieldsQueryProcessor)
        {
            _customersQueryProcessor = customersQueryProcessor;
            _ordersQueryProcessor = ordersQueryProcessor;
            _complaintsQueryProcessor = complaintsQueryProcessor;
            _participantsQueryProcessor = participantsQueryProcessor;
            _fieldsQueryProcessor = fieldsQueryProcessor;
        }

        public IEnumerable<OffendingCustomersReport> GetOffendingCustomersReport(DateTime? fromDate, DateTime? untilDate, int? complaintType)
        {
           throw new NotImplementedException();
        }

        public IEnumerable<CustomersActivityReport> GetCustomersActivityReport(string firstName, string lastName, int? minAge, int? maxAge, DateTime? fromDate, DateTime? untilDate)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UsingFieldsReport> GetUsingFieldsReport(int? fieldId, string fieldName, DateTime? fromDate, DateTime? untilDate)
        {
            var orders = _ordersQueryProcessor.Search(null, null, null, new int?[] { (int)Consts.Decodes.OrderStatus.Accepted }, null, null, fromDate, untilDate);
            var report = _fieldsQueryProcessor.Search(fieldId, fieldName, null).Select(x =>
             new UsingFieldsReport()
             {
                 FieldId = x.Id ?? 0,
                 FieldName = x.Name,
                 hours16_18Orders = orders.Where(f => x.Id == f.Field.Id && DateUtils.ConvertFromJavaScript(f.StartDate).Hour == 16).Count(),
                 hours18_20Orders = orders.Where(f => x.Id == f.Field.Id && DateUtils.ConvertFromJavaScript(f.StartDate).Hour == 18).Count(),
                 hours20_22Orders = orders.Where(f => x.Id == f.Field.Id && DateUtils.ConvertFromJavaScript(f.StartDate).Hour == 20).Count(),
                 WeekEndOrders = orders.Where(f => x.Id == f.Field.Id && (DateUtils.ConvertFromJavaScript(f.StartDate).DayOfWeek == DayOfWeek.Friday || DateUtils.ConvertFromJavaScript(f.StartDate).DayOfWeek == DayOfWeek.Saturday)).Count(),
                 WeekDayOrders = orders.Where(f => x.Id == f.Field.Id && !(DateUtils.ConvertFromJavaScript(f.StartDate).DayOfWeek == DayOfWeek.Friday || DateUtils.ConvertFromJavaScript(f.StartDate).DayOfWeek == DayOfWeek.Saturday)).Count()
             });


            return report;
        }
    }
}