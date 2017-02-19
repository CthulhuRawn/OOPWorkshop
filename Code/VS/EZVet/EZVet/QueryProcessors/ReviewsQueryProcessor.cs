using System.Collections.Generic;
using System.Linq;
using EZVet.DTOs;
using NHibernate;

namespace EZVet.QueryProcessors
{
    public interface IReviewsQueryProcessor
    {
        IEnumerable<Review> Search(int reviewedCustomerId);

        Review GetReview(int id);

        Review Save(Review review);

        Review Update(int id, Review review);
    }

    public class ReviewsQueryProcessor : DBAccessBase<Domain.Review>, IReviewsQueryProcessor
    {
        private IOwnersQueryProcessor _customersQueryProcessor;

        public ReviewsQueryProcessor(ISession session, IOwnersQueryProcessor customersQueryProcessor) : base(session)
        {
            _customersQueryProcessor = customersQueryProcessor;
        }

        public IEnumerable<Review> Search(int reviewedCustomerId)
        {
            return Query().Where(x => x.ReviewedCustomer.Id == reviewedCustomerId).ToList().Select(x => new Review().Initialize(x));
        }

        public Review GetReview(int id)
        {
            return new Review().Initialize(Get(id));
        }

        public Review Save(Review review)
        {
            var newReview = new Domain.Review
            {
                Date = review.Date,
                Title = review.Title,
                Description = review.Description
            };

            var persistedReview = Save(newReview);

            return new Review().Initialize(persistedReview);
        }

        // TODO consider delete
        public Review Update(int id, Review review) { return null; }
    }
}