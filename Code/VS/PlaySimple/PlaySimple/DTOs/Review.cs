using System;
using Domain;
using System.ComponentModel.DataAnnotations;
using PlaySimple.Validators;

namespace PlaySimple.DTOs
{
    public class Review : Entity<DTOs.Review, Domain.Review>
    {
        [MaxLength(20)]
        public virtual string Title { get; set; }

        [MaxLength(1000)]
        public virtual string Description { get; set; }

        public virtual DateTime Date { get; set; }

        [ExistsInDB(typeof(Domain.Customer))]
        public virtual Customer Reviewer { get; set; }

        [ExistsInDB(typeof(Domain.Customer))]
        public virtual Customer ReviewedCustomer { get; set; }

        public override Review Initialize(Domain.Review domain)
        {
            Id = domain.Id;
            Title = domain.Title;
            Description = domain.Description;
            Date = domain.Date;
            Reviewer = new DTOs.Customer().Initialize(domain.Reviewer);
            ReviewedCustomer = new DTOs.Customer().Initialize(domain.ReviewedCustomer); ;

            return this;
        }
    }
}
