using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using EZVet.Common;
using EZVet.DTOs;
using EZVet.Filters;
using EZVet.QueryProcessors;

namespace EZVet.Controllers
{
    [Authorize(Roles = Consts.Roles.Admin + "," + Consts.Roles.Owner + "," + Consts.Roles.Doctor)]
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
        public List<Order> SearchMyOrders(int? orderId = null, int? orderStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            var userId = currIdentity.UserId;
            int?[] statuses = null;
            if (orderStatusId.HasValue)
                statuses = new[] { orderStatusId };
            return _ordersQueryProcessor.Search(orderId, userId, null, statuses, fieldId, fieldName, fromDate, untilDate);
        }

        [Route("api/orders/search")]
        [HttpGet]
        public List<Order> Search(int? orderId = null, int? ownerId = null, string ownerName = null, int? orderStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            int?[] statuses = null;
            if (orderStatusId.HasValue)
                statuses = new[] { orderStatusId };
            return _ordersQueryProcessor.Search(orderId, ownerId, ownerName, statuses, fieldId, fieldName, fromDate, untilDate);
        }


        [Route("api/orders/availablestojoin")]
        [HttpGet]
        public List<Order> SearchAvailableOrdersToJoin(int? ownerId = null, string ownerName = null, int? orderId = null, int? orderStatusId = null, int? fieldId = null, string fieldName = null, DateTime? fromDate = null, DateTime? untilDate = null)
        {
            return _ordersQueryProcessor.SearchAvailableOrdersToJoin(ownerId, ownerName, orderId, orderStatusId, fieldId, fieldName, fromDate, untilDate).ToList();
        }

        // GET: api/Orders/5
        [HttpGet]
        public Order Get(int id)
        {
            var order = _ordersQueryProcessor.GetOrder(id);
            order.Participants = _participantsQueryProcessor.Search(null, null, new int?[] { (int)Consts.Decodes.InvitationStatus.Sent, (int)Consts.Decodes.InvitationStatus.Accepted }, 
                null, order.Id, null, null, null, null).ToList();
            return order;
        }

        // POST: api/Orders
        [HttpPost]
        [TransactionFilter]
        public Order Save([FromBody]Order order)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            var userId = currIdentity.UserId;
         
            return _ordersQueryProcessor.Save(order);
        }

        [HttpPut]
        [TransactionFilter]
        public Order Update([FromUri]int id, [FromBody]Order order)
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
        public IEnumerable<Participant> SearchParticipants(int orderId, int? customerId)
        {
            return null;
        }

        [HttpGet]
        [Route("api/orders/availables")]
        public List<Order> SearchAvailableOrders(int? fieldId = null, string fieldName = null, int? fieldType = null, DateTime? date = null)
        {
            return _ordersQueryProcessor.SearchOptionalOrders(fieldId, fieldName, fieldType, date??DateTime.Today);
        }

        
        [Route("api/orders/optionals")]
        [HttpGet]
        public List<Order> SearchOptionalsOrders(int? orderId = null, int? fieldId = null, int? fieldType = null, DateTime? date = null)
        {
            DateTime? dateTime = date.Value.Date;

            //if (date.HasValue)
            //    dateTime = DateUtils.ConvertFromJavaScript(date ?? 0);


            var optionals = _ordersQueryProcessor.SearchOptionalOrders(fieldId, null, fieldType, dateTime??DateTime.Today);

            if (orderId.HasValue)
            {
                var current = _ordersQueryProcessor.GetOrder(orderId ?? 0);

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
        public Order UpdatePraticipant([FromUri]int id, [FromBody]Participant participant)
        {
            _participantsQueryProcessor.Update(id, participant);
            return Get(participant.Order.Id??0);
        }

        [HttpGet]
        [TransactionFilter]
        [Route("api/orders/jointoorder")]
        public Participant JoinToOrder(int? orderId = null)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            var userId = currIdentity.UserId;

            var newParticipant = new Participant
            {
                Status = (int)Consts.Decodes.InvitationStatus.Sent,
                Date = DateTime.Now,
                
                Order = new Order
                {
                    Id = orderId
                }
            };
            return _participantsQueryProcessor.Save(newParticipant);
        }
    }
}
