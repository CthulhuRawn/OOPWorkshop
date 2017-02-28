using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    
    public class ReportsController : ApiController
    {
        private readonly IReportsQueryProcessor _reportsQueryProcessor;

        public ReportsController(IReportsQueryProcessor reportsQueryProcessor)
        {
            _reportsQueryProcessor = reportsQueryProcessor;
        }
        

        [HttpGet]
        [Route("api/reports/finance")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public List<FinanceReport> Finance(DateTime? startDate = null, DateTime? endDate = null, int? datePart = null)
        {
            var cookieValues = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var doctorId = -1;
            var ownerId = -1;
            if (cookieValues[1] == Consts.Roles.Doctor)
            {
                doctorId = int.Parse(cookieValues[0]);
            }
            else
            {
                ownerId = int.Parse(cookieValues[0]);
            }
            return _reportsQueryProcessor.GetFinanceReport(startDate, endDate, datePart, doctorId, ownerId).ToList();
        }
    }
}
