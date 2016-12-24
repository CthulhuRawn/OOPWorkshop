using PlaySimple.Filters;
using PlaySimple.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Linq;

namespace PlaySimple.Controllers
{
    [Authorize(Roles = Consts.Roles.Customer)]
    public class ParticipantsController : ApiController
    {
        private readonly IParticipantsQueryProcessor _participantsQueryProcessor;

        public ParticipantsController(IParticipantsQueryProcessor participantsQueryProcessor)
        {
            _participantsQueryProcessor = participantsQueryProcessor;
        }

        [HttpGet]
        public List<DTOs.Participant> SearchParticipants(int? ownerId = null, string ownerName = null, int? orderId = null, int? invitationStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            int userId = currIdentity.UserId;

            int?[] statuses = null;
            if (invitationStatusId.HasValue)
                statuses = new int?[] { invitationStatusId };

            return _participantsQueryProcessor.Search(userId, ownerId, statuses, ownerName, orderId, fieldId, fieldName, fromDate, untilDate).ToList();
        }

        [HttpGet]
        public DTOs.Participant Get(int id)
        {
            return _participantsQueryProcessor.GetParticipant(id);
        }

        [HttpPost]
        [TransactionFilter]
        public DTOs.Participant Save([FromBody]DTOs.Participant Participant)
        {
            return _participantsQueryProcessor.Save(Participant);
        }

        [HttpPut]
        [TransactionFilter]
        public DTOs.Participant Update([FromUri]int id, [FromBody]DTOs.Participant Participant)
        {
            return _participantsQueryProcessor.Update(id, Participant);
        }

        [HttpDelete]
        [TransactionFilter]
        public void Delete([FromUri]int id)
        {
            _participantsQueryProcessor.Delete(id);
        }
    }
}