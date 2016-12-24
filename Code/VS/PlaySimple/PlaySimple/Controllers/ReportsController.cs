using PlaySimple.DTOs;
using PlaySimple.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PlaySimple.Controllers
{
    [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Employee)]
    public class ReportsController : ApiController
    {
        private readonly IReportsQueryProcessor _reportsQueryProcessor;

        public ReportsController(IReportsQueryProcessor reportsQueryProcessor)
        {
            _reportsQueryProcessor = reportsQueryProcessor;
        }

        [HttpGet]
        [Route("api/reports/complaints")]
        public List<OffendingCustomersReport> GetOffendingCustomersReport(DateTime? fromDate = null, DateTime? untilDate = null, int? complaintType = null)
        {
            return _reportsQueryProcessor.GetOffendingCustomersReport(fromDate, untilDate, complaintType).ToList();
        }

        [HttpGet]
        [Route("api/reports/customers")]
        public List<CustomersActivityReport> GetCustomersActivityReport(string firstName = null, string lastName = null, int? minAge = null, int? maxAge = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            return _reportsQueryProcessor.GetCustomersActivityReport(firstName, lastName, minAge, maxAge, fromDate, untilDate).ToList();
        }

        [HttpGet]
        [Route("api/reports/fields")]
        public List<UsingFieldsReport> GetUsingFieldsReport(string fieldName = null, int? fieldId = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            return _reportsQueryProcessor.GetUsingFieldsReport(fieldId, fieldName, fromDate, untilDate).ToList();
        }
    }
}
