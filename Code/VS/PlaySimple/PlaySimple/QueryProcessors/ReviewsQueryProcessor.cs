using Domain;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace PlaySimple.QueryProcessors
{
    public interface IReviewsQueryProcessor
    {
        IEnumerable<DTOs.Review> Search(int reviewedCustomerId);

        DTOs.Review GetReview(int id);

        DTOs.Review Save(DTOs.Review review);

        DTOs.Review Update(int id, DTOs.Review review);
    }

    public class ReviewsQueryProcessor : DBAccessBase<Review>, IReviewsQueryProcessor
    {
        private CustomersQueryProcessor _customersQueryProcessor;

        public ReviewsQueryProcessor(ISession session, CustomersQueryProcessor customersQueryProcessor) : base(session)
        {
            _customersQueryProcessor = customersQueryProcessor;
        }

        public IEnumerable<DTOs.Review> Search(int reviewedCustomerId)
        {
            return Query().Where(x => x.ReviewedCustomer.Id == reviewedCustomerId).ToList().Select(x => new DTOs.Review().Initialize(x));
        }

        public DTOs.Review GetReview(int id)
        {
            return new DTOs.Review().Initialize(Get(id));
        }

        public DTOs.Review Save(DTOs.Review review)
        {
            Review newReview = new Review()
            {
                Date = review.Date,
                Title = review.Title,
                Description = review.Description,
                Reviewer = _customersQueryProcessor.Get(review.Reviewer.Id ?? 0),
                ReviewedCustomer = _customersQueryProcessor.Get(review.ReviewedCustomer.Id ?? 0)
            };

            Review persistedReview = Save(newReview);

            return new DTOs.Review().Initialize(persistedReview);
        }

        // TODO consider delete
        public DTOs.Review Update(int id, DTOs.Review review) { return null; }
    }
}