using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DAL;
using DTO;
using DTO.Enums;
using EZVet.Filters;

namespace EZVet.Controllers
{
    
    public class ReportsController : ApiController
    {
        private readonly IReportsDao _reportsDao;

        public ReportsController(IReportsDao reportsDao)
        {
            _reportsDao = reportsDao;
        }
        

        [HttpGet]
        [Route("api/reports/finance")]
        [AuthorizeRoles(  Roles.Owner,Roles.Doctor)]
        public List<FinanceReport> Finance([DateTimeParameter]DateTime? startDate = null, [DateTimeParameter]DateTime? endDate = null, int? datePart = null)
        {
            var cookieValues = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var doctorId = -1;
            var ownerId = -1;
            if (cookieValues[1] == Roles.Doctor.ToString())
            {
                doctorId = int.Parse(cookieValues[0]);
            }
            else
            {
                ownerId = int.Parse(cookieValues[0]);
            }
            return _reportsDao.GetFinanceReport(startDate, endDate, datePart, doctorId, ownerId).ToList();
        }

        [HttpGet]
        [Route("api/reports/perItem")]
        [AuthorizeRoles(  Roles.Doctor)]
        public List<ItemUsageReport> PerItem([DateTimeParameter]DateTime? startDate = null, [DateTimeParameter]DateTime? endDate = null, int? datePart = null, string itemName = "")
        {
            var doctorId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _reportsDao.GetPerItemReport(startDate, endDate, datePart, doctorId, itemName).ToList();
        }

        [HttpGet]
        [Route("api/reports/perType")]
        [AuthorizeRoles(  Roles.Doctor)]
        public List<AnimalTypeReport> PerType([DateTimeParameter]DateTime? startDate = null, [DateTimeParameter]DateTime? endDate = null, int? datePart = null, int? animalType = null)
        {
            var doctorId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _reportsDao.GetPerTypeReport(startDate, endDate, datePart, doctorId, animalType).ToList();
        }

        [HttpGet]
        [Route("api/reports/perAnimal")]
        [AuthorizeRoles(  Roles.Owner)]
        public List<AnimalNameReport> PerAnimal([DateTimeParameter]DateTime? startDate = null, [DateTimeParameter]DateTime? endDate = null, int? datePart = null, int? animalId = null)
        {
            var ownerId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _reportsDao.GetPerAnimalReport(startDate, endDate, datePart, ownerId, animalId).ToList();
        }

        [HttpGet]
        [Route("api/reports/visits")]
        [AuthorizeRoles( Roles.Owner,Roles.Doctor)]
        public List<VisitsReport> Visits(int? time = null)
        {
            var cookieValues = HttpContext.Current.Request.Cookies["UserId"].Value.Split(':');
            var doctorId = -1;
            var ownerId = -1;
            if (cookieValues[1] == Roles.Doctor.ToString())
            {
                doctorId = int.Parse(cookieValues[0]);
            }
            else
            {
                ownerId = int.Parse(cookieValues[0]);
            }
            return _reportsDao.GetVisitsReport(time, doctorId, ownerId).ToList();
        }
    }
}
