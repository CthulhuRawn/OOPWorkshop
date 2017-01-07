using System;
using System.ComponentModel.DataAnnotations;

namespace EZVet.DTOs
{
    public class CustomersActivityReport
    {
        public int CoustomerId { get; set; }

        [MaxLength(20)]
        public virtual string FirstName { get; set; }

        [MaxLength(20)]
        public virtual string LastName { get; set; }

        public virtual DateTime BirthDate { get; set; }

        public virtual long LastGameDate { get; set; }

        public virtual int NumberOfOrders { get; set; }

        public virtual int NumberOfCanceledOrders { get; set; }

        public virtual int NumberOfJoiningAsGuest { get; set; }
    }
}
