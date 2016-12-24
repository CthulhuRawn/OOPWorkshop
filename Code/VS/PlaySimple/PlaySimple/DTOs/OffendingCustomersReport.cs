using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaySimple.DTOs
{
    public class OffendingCustomersReport
    {
        public int CustomerId { get; set; }

        [MaxLength(20)]
        public virtual string FirstName { get; set; }

        [MaxLength(20)]
        public virtual string LastName { get; set; }

        public virtual int NumberOfComplaints{ get; set; }
    }
}
