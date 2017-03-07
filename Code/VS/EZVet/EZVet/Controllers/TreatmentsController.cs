using System.Web;
using System.Web.Http;
using DAL;
using DTO;
using DTO.Enums;
using EZVet.Filters;

namespace EZVet.Controllers
{
    public class TreatmentsController : ApiController
    {
        private readonly ITreatmentsDao _treatmentsDao;

        public TreatmentsController(ITreatmentsDao treatmentDao)
        {
            _treatmentsDao = treatmentDao;
        }

        [HttpPost]
        [AuthorizeRoles(Roles.Doctor)]
        [TransactionFilter]
        public TreatmentReport Save(TreatmentReport treatmentReport)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _treatmentsDao.Save(treatmentReport, userId);
        }

        [HttpGet]
        [AuthorizeRoles(Roles.Doctor, Roles.Owner)]
        public TreatmentReport Get(int petId, int treatmentId)
        {
            return _treatmentsDao.Get(treatmentId, petId);
        }

    }
}