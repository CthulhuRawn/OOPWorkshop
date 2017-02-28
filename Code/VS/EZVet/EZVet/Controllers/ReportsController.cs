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

        [HttpGet]
        [Route("api/reports/perItem")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Doctor)]
        public List<ItemUsageReport> PerItem(DateTime? startDate = null, DateTime? endDate = null, int? datePart = null, string itemName = "")
        {
            var doctorId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _reportsQueryProcessor.GetPerItemReport(startDate, endDate, datePart, doctorId, itemName).ToList();
        }

        [HttpGet]
        [Route("api/reports/perType")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Doctor)]
        public List<AnimalTypeReport> PerType(DateTime? startDate = null, DateTime? endDate = null, int? datePart = null, int? animalType = null)
        {
            var doctorId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _reportsQueryProcessor.GetPerTypeReport(startDate, endDate, datePart, doctorId, animalType).ToList();
        }

        [HttpGet]
        [Route("api/reports/perAnimal")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner)]
        public List<AnimalNameReport> PerAnimal(DateTime? startDate = null, DateTime? endDate = null, int? datePart = null, int? animalId = null)
        {
            var ownerId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _reportsQueryProcessor.GetPerAnimalReport(startDate, endDate, datePart, ownerId, animalId).ToList();
        }

        [HttpGet]
        [Route("api/reports/visits")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
        public List<VisitsReport> Visits(int? time)
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
            return _reportsQueryProcessor.GetVisitsReport(time, doctorId, ownerId).ToList();
        }
    }
}
