using System;
using System.ComponentModel.DataAnnotations;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Review : Entity<DTOs.Review, Domain.Review>
    {
        [MaxLength(20)]
        public virtual string Title { get; set; }

        [MaxLength(1000)]
        public virtual string Description { get; set; }

        public virtual DateTime Date { get; set; }

       

        public override Review Initialize(Domain.Review domain)
        {
            Id = domain.Id;
            Title = domain.Title;
            Description = domain.Description;
            Date = domain.Date;
           
            return this;
        }
    }
}
