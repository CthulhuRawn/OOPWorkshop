using Domain;
using LinqKit;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.QueryProcessors
{
    public interface IComplaintsQueryProcessor
    {
        // TODO changed, add to doc
        IEnumerable<DTOs.Complaint> Search(int? customerId, DateTime? fromDate, DateTime? untilDate, int? complaintType);

        DTOs.Complaint GetComplaint(int id);

        DTOs.Complaint Save(DTOs.Complaint complaint);
    }

    public class ComplaintsQueryProcessor : DBAccessBase<Complaint>, IComplaintsQueryProcessor
    {
        ICustomersQueryProcessor _customersQueryProcessor;
        IDecodesQueryProcessor _decodesQueryProcessor;

        public ComplaintsQueryProcessor(ISession session, IDecodesQueryProcessor decodesQueryProcessor, ICustomersQueryProcessor customersQueryProcessor) : base(session)
        {
            _decodesQueryProcessor = decodesQueryProcessor;
            _customersQueryProcessor = customersQueryProcessor;
        }

        public IEnumerable<DTOs.Complaint> Search(int? customerId, DateTime? fromDate, DateTime? untilDate, int? complaintType)
        {
            var filter = PredicateBuilder.New<Complaint>(x => true);

            if (customerId.HasValue)
                filter.And(x => x.OffendingCustomer.Id == customerId);

            if (fromDate.HasValue)
                filter.And(x => x.Date >= fromDate);

            if (untilDate.HasValue)
                filter.And(x => x.Date <= untilDate);

            if (complaintType.HasValue)
                filter.And(x => x.Type == _decodesQueryProcessor.Get<ComplaintTypeDecode>(complaintType??0));

            return Query().Where(filter).ToList().Select(x => new DTOs.Complaint().Initialize(x));
        }

        public DTOs.Complaint GetComplaint(int id)
        {
            return new DTOs.Complaint().Initialize(Get(id));
        }

        public DTOs.Complaint Save(DTOs.Complaint complaint)
        {
            Complaint newComplaint = new Complaint()
            {
                OffendingCustomer = ((DBAccessBase<Customer>)_customersQueryProcessor).Get(complaint.OffendingCustomer.Id ?? 0),
                OffendedCustomer = ((DBAccessBase<Customer>)_customersQueryProcessor).Get(complaint.OffendedCustomer.Id ?? 0),
                Description = complaint.Description,
                Type = _decodesQueryProcessor.Get<ComplaintTypeDecode>(complaint.Type),
                Date = complaint.Date
                
            };

            Complaint persistedComplaint = Save(newComplaint);

            return new DTOs.Complaint().Initialize(persistedComplaint);
        }
    }
}