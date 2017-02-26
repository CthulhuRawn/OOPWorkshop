using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    public class TreatmentsController : ApiController
    {
        private readonly ITreatmentsQueryProcessor _treatmentsQueryProcessor;

        public TreatmentsController(ITreatmentsQueryProcessor treatmentQueryProcessor)
        {
            _treatmentsQueryProcessor = treatmentQueryProcessor;
        }

        [HttpPost]
        [Route("api/treatments/save")]
        [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Doctor)]
        [TransactionFilter]
        public TreatmentReport Save(TreatmentReport treatmentReport)
        {
            var userId = int.Parse(HttpContext.Current.Request.Cookies["UserId"].Value.Split(':')[0]);
            return _treatmentsQueryProcessor.Save(treatmentReport, userId);
        }
    }
}