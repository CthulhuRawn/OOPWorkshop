using System;
using System.Collections.Generic;
using Domain;
using PlaySimple.Validators;
using PlaySimple.Common;

namespace PlaySimple.DTOs
{
    public class Order : Entity<DTOs.Order, Domain.Order>
    {
        //[NotInPast]
        public virtual long StartDate { get; set; }

        [ExistsInDB(typeof(Domain.Customer))]
        public virtual Customer Owner { get; set; }

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
            Owner = new DTOs.Customer().Initialize(domain.Owner);
            PlayersNumber = domain.PlayersNumber;
            Status = domain.Status.Id;
            Field = new DTOs.Field().Initialize(domain.Field);

            return this;
        }
    }
}
