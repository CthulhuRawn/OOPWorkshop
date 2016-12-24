using PlaySimple.Filters;
using System.Linq;
using PlaySimple.QueryProcessors;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using PlaySimple.Common;

namespace PlaySimple.Controllers
{
    [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Employee + "," + Consts.Roles.Customer)]
    public class OrdersController : ApiController
    {
        private readonly IOrdersQueryProcessor _ordersQueryProcessor;
        private readonly IParticipantsQueryProcessor _participantsQueryProcessor;

        public OrdersController(IOrdersQueryProcessor ordersQueryProcessor, IParticipantsQueryProcessor participantsQueryProcessor)
        {
            _ordersQueryProcessor = ordersQueryProcessor;
            _participantsQueryProcessor = participantsQueryProcessor;
        }

        [Route("api/orders/searchownedorders")]
        [HttpGet]
        public List<DTOs.Order> SearchMyOrders(int? orderId = null, int? orderStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            int userId = currIdentity.UserId;
            int?[] statuses = null;
            if (orderStatusId.HasValue)
                statuses = new int?[] { orderStatusId };
            return _ordersQueryProcessor.Search(orderId, userId, null, statuses, fieldId, fieldName, fromDate, untilDate);
        }

        [Route("api/orders/search")]
        [HttpGet]
        public List<DTOs.Order> Search(int? orderId = null, int? ownerId = null, string ownerName = null, int? orderStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            int?[] statuses = null;
            if (orderStatusId.HasValue)
                statuses = new int?[] { orderStatusId };
            return _ordersQueryProcessor.Search(orderId, ownerId, ownerName, statuses, fieldId, fieldName, fromDate, untilDate);
        }


        [Route("api/orders/availablestojoin")]
        [HttpGet]
        public List<DTOs.Order> SearchAvailableOrdersToJoin(int? ownerId = null, string ownerName = null, int? orderId = null, int? orderStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            return _ordersQueryProcessor.SearchAvailableOrdersToJoin(ownerId, ownerName, orderId, orderStatusId, fieldId, fieldName, fromDate, untilDate).ToList();
        }

        // GET: api/Orders/5
        [HttpGet]
        public DTOs.Order Get(int id)
        {
            DTOs.Order order = _ordersQueryProcessor.GetOrder(id);
            order.Participants = _participantsQueryProcessor.Search(null, null, new int?[] { (int)Consts.Decodes.InvitationStatus.Sent, (int)Consts.Decodes.InvitationStatus.Accepted }, 
                null, order.Id, null, null, null, null).ToList();
            return order;
        }

        // POST: api/Orders
        [HttpPost]
        [TransactionFilter]
        public DTOs.Order Save([FromBody]DTOs.Order order)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            int userId = currIdentity.UserId;
            order.Owner = new DTOs.Customer()
            {
                Id = userId
            };
            return _ordersQueryProcessor.Save(order);
        }

        [HttpPut]
        [TransactionFilter]
        public DTOs.Order Update([FromUri]int id, [FromBody]DTOs.Order order)
        {
            if(order.Status == (int)Consts.Decodes.OrderStatus.Canceled)
            {
                if (order.Participants != null)
                {
                    foreach (var participant in order.Participants)
                    {
                        participant.Status = (int)Consts.Decodes.InvitationStatus.Rejected;
                        _participantsQueryProcessor.Update(participant.Id ?? 0, participant);
                    }
                }
            }
            return _ordersQueryProcessor.Update(id, order);
        }

        [HttpGet]
        public IEnumerable<DTOs.Participant> SearchParticipants(int orderId, int? customerId)
        {
            return null;
        }

        [HttpGet]
        [Route("api/orders/availables")]
        public List<DTOs.Order> SearchAvailableOrders(int? fieldId = null, string fieldName = null, int? fieldType = null, DateTime? date = null)
        {
            return _ordersQueryProcessor.SearchOptionalOrders(fieldId, fieldName, fieldType, date??DateTime.Today);
        }

        
        [Route("api/orders/optionals")]
        [HttpGet]
        public List<DTOs.Order> SearchOptionalsOrders(int? orderId = null, int? fieldId = null, int? fieldType = null, DateTime? date = null)
        {
            DateTime? dateTime = date.Value.Date;

            //if (date.HasValue)
            //    dateTime = DateUtils.ConvertFromJavaScript(date ?? 0);


            List<DTOs.Order> optionals = _ordersQueryProcessor.SearchOptionalOrders(fieldId, null, fieldType, dateTime??DateTime.Today);

            if (orderId.HasValue)
            {
                DTOs.Order current = _ordersQueryProcessor.GetOrder(orderId ?? 0);

                if (current.Field.Id == fieldId && DateUtils.ConvertFromJavaScript(current.StartDate).Date == dateTime.Value.Date)
                {
                    optionals.Add(current);
                }
                
            }

            return optionals;
        }

        [HttpPut]
        [TransactionFilter]
        [Route("api/orders/updatepraticipant")]
        public DTOs.Order UpdatePraticipant([FromUri]int id, [FromBody]DTOs.Participant participant)
        {
            _participantsQueryProcessor.Update(id, participant);
            return Get(participant.Order.Id??0);
        }

        [HttpGet]
        [TransactionFilter]
        [Route("api/orders/jointoorder")]
        public DTOs.Participant JoinToOrder(int? orderId = null)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            int userId = currIdentity.UserId;

            DTOs.Participant newParticipant = new DTOs.Participant()
            {
                Status = (int)Consts.Decodes.InvitationStatus.Sent,
                Date = DateTime.Now,
                Customer = new DTOs.Customer()
                {
                    Id = userId
                },
                Order = new DTOs.Order()
                {
                    Id = orderId
                }
            };
            return _participantsQueryProcessor.Save(newParticipant);
        }
    }
}
