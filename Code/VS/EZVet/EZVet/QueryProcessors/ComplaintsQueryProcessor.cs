using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using LinqKit;
using NHibernate;
using Complaint = EZVet.DTOs.Complaint;

namespace EZVet.QueryProcessors
{
    public interface IComplaintsQueryProcessor
    {
        // TODO changed, add to doc
        IEnumerable<Complaint> Search(int? customerId, DateTime? fromDate, DateTime? untilDate, int? complaintType);

        Complaint GetComplaint(int id);

        Complaint Save(Complaint complaint);
    }

    public class ComplaintsQueryProcessor : DBAccessBase<Domain.Complaint>, IComplaintsQueryProcessor
    {
        IOwnersQueryProcessor _customersQueryProcessor;
        IDecodesQueryProcessor _decodesQueryProcessor;

        public ComplaintsQueryProcessor(ISession session, IDecodesQueryProcessor decodesQueryProcessor, IOwnersQueryProcessor customersQueryProcessor) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
            _customersQueryProcessor = customersQueryProcessor;
        }

        public IEnumerable<Complaint> Search(int? customerId, DateTime? fromDate, DateTime? untilDate, int? complaintType)
        {
            var filter = PredicateBuilder.New<Domain.Complaint>(x => true);

            if (customerId.HasValue)
                filter.And(x => x.OffendingCustomer.Id == customerId);

            if (fromDate.HasValue)
                filter.And(x => x.Date >= fromDate);

            if (untilDate.HasValue)
                filter.And(x => x.Date <= untilDate);

            if (complaintType.HasValue)
                filter.And(x => x.Type == _decodesQueryProcessor.Get<ComplaintTypeDecode>(complaintType??0));

            return Query().Where(filter).ToList().Select(x => new Complaint().Initialize(x));
        }

        public Complaint GetComplaint(int id)
        {
            return new Complaint().Initialize(Get(id));
        }

        public Complaint Save(Complaint complaint)
        {
            var newComplaint = new Domain.Complaint
            {
               
                Description = complaint.Description,
                Type = _decodesQueryProcessor.Get<ComplaintTypeDecode>(complaint.Type),
                Date = complaint.Date
                
            };

            var persistedComplaint = Save(newComplaint);

            return new Complaint().Initialize(persistedComplaint);
        }
    }
}