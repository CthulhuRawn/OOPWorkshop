using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    [AuthorizeRoles(Consts.Roles.Doctor)]
    public class ParticipantsController : ApiController
    {
        private readonly IParticipantsQueryProcessor _participantsQueryProcessor;

        public ParticipantsController(IParticipantsQueryProcessor participantsQueryProcessor)
        {
            _participantsQueryProcessor = participantsQueryProcessor;
        }

        [HttpGet]
        public List<Participant> SearchParticipants(int? ownerId = null, string ownerName = null, int? orderId = null, int? invitationStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            var userId = currIdentity.UserId;

            int?[] statuses = null;
            if (invitationStatusId.HasValue)
                statuses = new[] { invitationStatusId };

            return _participantsQueryProcessor.Search(userId, ownerId, statuses, ownerName, orderId, fieldId, fieldName, fromDate, untilDate).ToList();
        }

        [HttpGet]
        public Participant Get(int id)
        {
            return _participantsQueryProcessor.GetParticipant(id);
        }

        [HttpPost]
        [TransactionFilter]
        public Participant Save([FromBody]Participant Participant)
        {
            return _participantsQueryProcessor.Save(Participant);
        }

        [HttpPut]
        [TransactionFilter]
        public Participant Update([FromUri]int id, [FromBody]Participant Participant)
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