using System;
using System.ComponentModel.DataAnnotations;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Complaint : Entity<DTOs.Complaint, Domain.Complaint>
    {
        [MaxLength(1000)]
        public virtual string Description { get; set; }

        [IsEnumOfType(typeof(Consts.Decodes.ComplaintType))]
        public virtual int? Type { get; set; }

        [NotInFutureAttribute]
        public virtual DateTime Date { get; set; }


        public override Complaint Initialize(Domain.Complaint domain)
        {
            Id = domain.Id;
            Description = domain.Description;
            Type = domain.Type.Id;
            Date = domain.Date;

            return this;
        }
    }
}
