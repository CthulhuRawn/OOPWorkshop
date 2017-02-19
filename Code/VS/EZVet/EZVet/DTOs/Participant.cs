using System;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Participant : Entity<Participant , Domain.Participant>
    {
         public virtual DateTime Date { get; set; }

        [ExistsInDB(typeof(Domain.Order))]
        public virtual Order Order { get; set; }

        [IsEnumOfType(typeof(Consts.Decodes.InvitationStatus))]
        public virtual int? Status { get; set; }

        public override Participant Initialize(Domain.Participant domain)
        {
            Id = domain.Id;
           
            Date = domain.Date;
            Order = new Order().Initialize(domain.Order);
            Status = domain.Status.Id;

            return this;
        }
    }
}
