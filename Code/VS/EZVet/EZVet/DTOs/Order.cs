using System.Collections.Generic;
using EZVet.Common;
using EZVet.Validators;

namespace EZVet.DTOs
{
    public class Order : Entity<DTOs.Order, Domain.Order>
    {
        //[NotInPast]
        public virtual long StartDate { get; set; }
        

        [Above(-1)]
        public virtual int PlayersNumber { get; set; }

        [IsEnumOfType(typeof(Consts.Decodes.OrderStatus))]
        public virtual int? Status { get; set; }

        public virtual Field Field { get; set; }

        // TODO REMOVE ListExistsInDb
        //[ListExistsInDb(typeof(Domain.Participant))]
        public virtual IList<DTOs.Participant> Participants { get; set; }

        public override Order Initialize(Domain.Order domain)
        {
            Id = domain.Id;
            StartDate = DateUtils.ConvertToJavaScript(domain.StartDate);

            PlayersNumber = domain.PlayersNumber;
            Status = domain.Status.Id;
            Field = new DTOs.Field().Initialize(domain.Field);

            return this;
        }
    }
}
