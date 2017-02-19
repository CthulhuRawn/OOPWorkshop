using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Domain;
using EZVet.Common;
using EZVet.Filters;
using LinqKit;
using NHibernate;
using Field = EZVet.DTOs.Field;
using Order = EZVet.DTOs.Order;

namespace EZVet.QueryProcessors
{
    public interface IOrdersQueryProcessor
    {
        List<Order> Search(int? orderId, int? ownerId, string ownerName, int?[] orderStatusIds, int? fieldId, string fieldName, DateTime? startDate, DateTime? endDate);

        Order GetOrder(int id);

        Order Save(Order order);

        Order Update(int id, Order order);

        List<Order> SearchOptionalOrders(int? fieldId, string fieldName, int? fieldTypeId, DateTime date);

        IEnumerable<Order> SearchAvailableOrdersToJoin(int? ownerId, string ownerName, int? orderId, int? orderStatusId, int? fieldId, string fieldName, DateTime? startDate, DateTime? endDate);
    }


    public class OrdersQueryProcessor : DBAccessBase<Domain.Order>, IOrdersQueryProcessor
    {
        private IOwnersQueryProcessor _customersQueryProcessor;
        private FieldsQueryProcessor _fieldsQueryProcessor;
        private IDecodesQueryProcessor _decodesQueryProcessor;
        private IParticipantsQueryProcessor _participantsQueryProcessor;

        public OrdersQueryProcessor(ISession session, IOwnersQueryProcessor customersQueryProcessor, FieldsQueryProcessor fieldsQueryProcessor,
            IDecodesQueryProcessor decodesQueryProcessor) : base(session)
        {
            _customersQueryProcessor = customersQueryProcessor;
            _fieldsQueryProcessor = fieldsQueryProcessor;
            _decodesQueryProcessor = decodesQueryProcessor;
            _participantsQueryProcessor = new ParticipantsQueryProcessor(session, customersQueryProcessor, this, decodesQueryProcessor);
        }

        public List<Order> Search(int? orderId, int? ownerId, string ownerName, int?[] orderStatusIds, int? fieldId, string fieldName, DateTime? startDate, DateTime? endDate)
        {
            var filter = PredicateBuilder.New<Domain.Order>(x => true);

            if (orderId.HasValue)
            {
                filter.And(x => x.Id == orderId);
            }

            if (ownerId.HasValue)
            {
                filter.And(x => x.Owner.Id == ownerId);
            }

            if (orderStatusIds != null)
            {
                filter.And(x => orderStatusIds.Contains(x.Status.Id));
            }

            if (fieldId.HasValue)
            {
                filter.And(x => x.Field.Id == fieldId);
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                filter.And(x => x.Field.Name.Contains(fieldName));
            }

            if (startDate.HasValue)
            {
                filter.And(x => x.StartDate >= startDate);
            }

            if (!string.IsNullOrEmpty(ownerName))
            {
                var names = ownerName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (names.Length == 1)
                {
                    filter.And(x => x.Owner.FirstName.Contains(ownerName) || x.Owner.LastName.Contains(ownerName));
                }else if (names.Length == 2)
                {
                    filter.And(x => x.Owner.FirstName.Contains(names[0]) || x.Owner.LastName.Contains(names[1]));
                }
            }

            if (endDate.HasValue)
            {
                DateTime? calcEndDate = endDate.Value.AddDays(1);
                filter.And(x => x.StartDate <= calcEndDate);
            }


            var result = Query().Where(filter).ToList().Select(x => new Order().Initialize(x));

            return result.ToList();
        }

        public Order GetOrder(int id)
        {
            return new Order().Initialize(Get(id));
        }

        public Order Save(Order order)
        {
            // TODO remove EndDate from Order
            var newOrder = new Domain.Order
            {
                
                StartDate = DateUtils.ConvertFromJavaScript(order.StartDate),
                Field = _fieldsQueryProcessor.Get(order.Field.Id ?? 0),
                PlayersNumber = order.PlayersNumber,
                Status = _decodesQueryProcessor.Get<OrderStatusDecode>(order.Status),
                Participants = new List<Participant>()
            };

            var persistedOrder = Save(newOrder);

            return new Order().Initialize(persistedOrder);
        }

        // Owner can be changed.
        public Order Update(int id, Order order)
        {
            var existingOrder = Get(id);

            if (order.Status != null)
                existingOrder.Status = _decodesQueryProcessor.Get<OrderStatusDecode>(order.Status);

            if (order.Field != null)
                existingOrder.Field = _fieldsQueryProcessor.Get(order.Field.Id ?? 0);

            if (order.PlayersNumber != 0)
                existingOrder.PlayersNumber = order.PlayersNumber;

            existingOrder.StartDate = DateUtils.ConvertFromJavaScript(order.StartDate);

            Update(id, existingOrder);

            return new Order().Initialize(existingOrder);
        }

        // TODO add to doc date is mandator
        public List<Order> SearchOptionalOrders(int? fieldId, string fieldName, int? fieldTypeId, DateTime date)
        {
            IList<Field> fields = _fieldsQueryProcessor.Search(fieldId, fieldName, fieldTypeId).ToList();
            var possibleDate = DateUtils.PossibleDateOrders(date);

            var possibleEvent = from field in _fieldsQueryProcessor.Search(fieldId, fieldName, fieldTypeId).ToList()
                                from dateStart in DateUtils.PossibleDateOrders(date)
                                select new { field, dateStart };

            var statuses = new int?[] { (int)Consts.Decodes.OrderStatus.Accepted, (int)Consts.Decodes.OrderStatus.Sent };

            var order = Search(null, null, null, statuses, null, null, date, date);

            var possibleOrders = possibleEvent.Where(a => !order.Any(r => r.StartDate == DateUtils.ConvertToJavaScript(a.dateStart) & r.Field.Id == a.field.Id)).
                ToList().Select(possibleOrder => new Order
                {
                    Field = possibleOrder.field,
                    StartDate = DateUtils.ConvertToJavaScript(possibleOrder.dateStart)
                });

            return possibleOrders.ToList();
        }

        public IEnumerable<Order> SearchAvailableOrdersToJoin(int? ownerId, string ownerName, int? orderId, int? orderStatusId, int? fieldId, string fieldName, DateTime? startDate, DateTime? endDate)
        {
            var currPrincipal = HttpContext.Current.User as ClaimsPrincipal;
            var currIdentity = currPrincipal.Identity as BasicAuthenticationIdentity;
            var userId = currIdentity.UserId;

            var filter = PredicateBuilder.New<Domain.Order>(x => x.Owner.Id != userId);

            if (orderId.HasValue)
            {
                filter.And(x => x.Id == orderId);
            }

            if (!string.IsNullOrEmpty(ownerName))
            {
                var names = ownerName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (names.Length == 1)
                {
                    filter.And(x => x.Owner.FirstName.Contains(ownerName) || x.Owner.LastName.Contains(ownerName));
                }
                else if (names.Length == 2)
                {
                    filter.And(x => x.Owner.FirstName.Contains(names[0]) || x.Owner.LastName.Contains(names[1]));
                }
            }
            if (ownerId.HasValue)
            {
                filter.And(x => x.Owner.Id == ownerId);
            }

            if (orderStatusId.HasValue)
            {
                filter.And(x => orderStatusId == x.Status.Id);
            }else
            {
                var statuses = new int?[] { (int)Consts.Decodes.OrderStatus.Accepted, (int)Consts.Decodes.OrderStatus.Sent };
                filter.And(x => statuses.Contains(x.Status.Id));
            }

            if (fieldId.HasValue)
            {
                filter.And(x => x.Field.Id == fieldId);
            }

            if (!string.IsNullOrEmpty(fieldName))
            {
                filter.And(x => x.Field.Name.Contains(fieldName));
            }

            if (startDate.HasValue)
            {
                filter.And(x => x.StartDate >= startDate);
            }

            if (endDate.HasValue)
            {
                DateTime? calcEndDate = endDate.Value.AddDays(1);
                filter.And(x => x.StartDate <= calcEndDate);
            }

            var allResult = Query().Where(filter).ToList().Select(x => new Order().Initialize(x));

            var finalResult = new List<Order>();

            foreach (var item in allResult)
            {
                var participants = _participantsQueryProcessor.Search(null, null, null, null, item.Id, null, null, null, null);
                
               
            }
            return finalResult;
        }
    }
}