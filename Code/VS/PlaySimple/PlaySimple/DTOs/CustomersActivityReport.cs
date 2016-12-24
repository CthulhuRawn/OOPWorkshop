using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySimple.DTOs
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
