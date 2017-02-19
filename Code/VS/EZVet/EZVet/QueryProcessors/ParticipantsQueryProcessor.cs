using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using LinqKit;
using NHibernate;
using Participant = EZVet.DTOs.Participant;

namespace EZVet.QueryProcessors
{
    public interface IParticipantsQueryProcessor
    {
        IEnumerable<Participant> Search(int? customerId, int? ownerId, int?[] statusIds, string ownerName, int? orderId, int? fieldId, string fieldName, DateTime? fromDate, DateTime? untilDate);

        Participant GetParticipant(int id);

        Participant Save(Participant participant);

        Participant Update(int id, Participant participant);

        void Delete(int id);
    }

    public class ParticipantsQueryProcessor : DBAccessBase<Domain.Participant>, IParticipantsQueryProcessor
    {
        private IOwnersQueryProcessor _customersQueryProcessor;
        private OrdersQueryProcessor _ordersQueryProcessor;
        private IDecodesQueryProcessor _decodesQueryProcessor;

        public ParticipantsQueryProcessor(ISession session, IOwnersQueryProcessor customersQueryProcessor, OrdersQueryProcessor ordersQueryProcessor, IDecodesQueryProcessor decodesQueryProcessor) : base(session)
        {
            _customersQueryProcessor = customersQueryProcessor;
            _ordersQueryProcessor = ordersQueryProcessor;
            _decodesQueryProcessor = decodesQueryProcessor;
        }

        //public IEnumerable<DTOs.Participant> Search(int? orderId, int? customerId, int?[] statusIds)
        //{
        //    var filter = PredicateBuilder.New<Participant>(x => true);

        //    if (orderId.HasValue)
        //    {
        //        filter.And(x => x.Order.Id == orderId);
        //    }

        //    if (customerId.HasValue)
        //    {
        //        filter.And(x => x.Customer.Id == customerId);
        //    }

        //    if (statusIds != null)
        //    {
        //        filter.And(x => statusIds.Contains(x.Status.Id));
        //    }

        //    var result =  Query().Where(filter).Select(x => new DTOs.Participant().Initialize(x));
        //    return result;

        //}

        public Participant GetParticipant(int id)
        {
            return new Participant().Initialize(Get(id));
        }

        public Participant Save(Participant participant)
        {
            var newParticipant = new Domain.Participant
            {
                
                Date = participant.Date,
                Order = _ordersQueryProcessor.Get(participant.Order.Id ?? 0),
                Status = _decodesQueryProcessor.Get<InvitationStatusDecode>(participant.Status)
            };

            var persistedParticipant = Save(newParticipant);

            return new Participant().Initialize(persistedParticipant);
        }

        // Only status can be changed
        public Participant Update(int id, Participant participant)
        {
            var existingParticipant = Get(id);

            existingParticipant.Status = _decodesQueryProcessor.Get<InvitationStatusDecode>(participant.Status);

            Update(id, existingParticipant);

            return new Participant().Initialize(existingParticipant);
        }

        public IEnumerable<Participant> Search(int? customerId, int? ownerId, int?[] statusIds, string ownerName, int? orderId, int? fieldId, string fieldName, DateTime? fromDate, DateTime? untilDate)
        {
            var filter = PredicateBuilder.New<Domain.Participant>(x => true);

            if (ownerId.HasValue)
            {
                filter.And(x => x.Order.Owner.Id == ownerId);
            }

            if (customerId.HasValue)
            {
                filter.And(x => x.Customer.Id == customerId);
            }

            if (statusIds != null)
            {
                filter.And(x => statusIds.Contains(x.Status.Id));
            }

            if (!string.IsNullOrEmpty(ownerName))
            {
                var names = ownerName.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (names.Length == 1)
                {
                    filter.And(x => x.Order.Owner.FirstName.Contains(ownerName) || x.Order.Owner.LastName.Contains(ownerName));
                }else if (names.Length == 2)
                {
                    filter.And(x => x.Order.Owner.FirstName.Contains(names[0]) || x.Order.Owner.LastName.Contains(names[1]));
                }
            }

            if (orderId.HasValue)
            {
                filter.And(x => x.Order.Id == orderId);
            }

            if (fieldId.HasValue)
            {
                filter.And(x => x.Order.Field.Id == fieldId);
            }

            if (fieldName != null)
            {
                filter.And(x => x.Order.Field.Name.Contains(fieldName));
            }

            if (fromDate.HasValue)
            {
                filter.And(x => x.Order.StartDate >= fromDate);
            }

            if (untilDate.HasValue)
            {
                DateTime? calcEndDate = untilDate.Value.AddDays(1);
                filter.And(x => x.Order.StartDate <= calcEndDate);
            }

            var result = Query().Where(filter).ToList().Select(x => new Participant().Initialize(x));
            return result;

        }

        public void Delete(int id)
        {
            Delete(new Domain.Participant { Id = id });
        }
    }
}