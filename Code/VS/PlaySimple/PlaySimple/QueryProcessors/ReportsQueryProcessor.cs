using Domain;
using PlaySimple.Common;
using PlaySimple.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public interface IReportsQueryProcessor
    {
        IEnumerable<OffendingCustomersReport> GetOffendingCustomersReport(DateTime? fromDate, DateTime? untilDate, int? complaintType);
        IEnumerable<CustomersActivityReport> GetCustomersActivityReport(string firstName, string lastName, int? minAge, int? maxAge, DateTime? fromDate, DateTime? untilDate);
        IEnumerable<UsingFieldsReport> GetUsingFieldsReport(int? fieldId, string fieldName, DateTime? fromDate, DateTime? untilDate);
    }

    public class ReportsQueryProcessor : IReportsQueryProcessor
    {
        private readonly ICustomersQueryProcessor _customersQueryProcessor;
        private readonly IComplaintsQueryProcessor _complaintsQueryProcessor;
        private readonly IOrdersQueryProcessor _ordersQueryProcessor;
        private readonly IParticipantsQueryProcessor _participantsQueryProcessor;
        private readonly IFieldsQueryProcessor _fieldsQueryProcessor;

        public ReportsQueryProcessor(ICustomersQueryProcessor customersQueryProcessor, IOrdersQueryProcessor ordersQueryProcessor, IComplaintsQueryProcessor complaintsQueryProcessor, IParticipantsQueryProcessor participantsQueryProcessor, IFieldsQueryProcessor fieldsQueryProcessor)
        {
            _customersQueryProcessor = customersQueryProcessor;
            _ordersQueryProcessor = ordersQueryProcessor;
            _complaintsQueryProcessor = complaintsQueryProcessor;
            _participantsQueryProcessor = participantsQueryProcessor;
            _fieldsQueryProcessor = fieldsQueryProcessor;
        }

        public IEnumerable<OffendingCustomersReport> GetOffendingCustomersReport(DateTime? fromDate, DateTime? untilDate, int? complaintType)
        {
            var complaintsGrouping = _complaintsQueryProcessor.Search(null, fromDate, untilDate, complaintType).GroupBy(x => x.OffendingCustomer.Id, x => x.OffendedCustomer.Id);
            List<OffendingCustomersReport> offendingList = new List<OffendingCustomersReport>();

            foreach (var item in complaintsGrouping)
            {
                DTOs.Customer customer = _customersQueryProcessor.GetCustomer(item.Key ?? 0);
                offendingList.Add(new OffendingCustomersReport()
                        {
                            CustomerId = customer.Id??0,
                            FirstName = customer.FirstName,
                            LastName = customer.LastName,
                            NumberOfComplaints = item.Count()
                        });
            }
            return offendingList;
        }

        public IEnumerable<CustomersActivityReport> GetCustomersActivityReport(string firstName, string lastName, int? minAge, int? maxAge, DateTime? fromDate, DateTime? untilDate)
        {
            var customers = _customersQueryProcessor.Search(firstName, lastName, minAge, maxAge, null, null);
            var report = customers.Select(x => new CustomersActivityReport
                {
                    CoustomerId = x.Id ?? 0,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    BirthDate = x.BirthDate,
                    NumberOfOrders = _ordersQueryProcessor.Search(null, x.Id, null, new int?[] { (int)Consts.Decodes.OrderStatus.Accepted }, null, null, fromDate, untilDate).Count(),
                    NumberOfCanceledOrders = _ordersQueryProcessor.Search(null, x.Id, null, new int?[] { (int)Consts.Decodes.OrderStatus.Canceled }, null, null, fromDate, untilDate).Count(),
                    NumberOfJoiningAsGuest = _participantsQueryProcessor.Search(x.Id, null, new int?[] { (int)Consts.Decodes.InvitationStatus.Accepted },
                    null, null, null, null, null, null).Count()
                }).ToArray();

            for (int i = 0; i < report.Count(); i++)
            {
                var item = report[i];
                var itemOrder = _ordersQueryProcessor.Search(null, item.CoustomerId, null, new int?[] { (int)Consts.Decodes.OrderStatus.Accepted }, null, null, null, null);

                if (itemOrder.Count != 0)
                {
                    item.LastGameDate = itemOrder.Max(x => x.StartDate);
                }
            }         

            return report;
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