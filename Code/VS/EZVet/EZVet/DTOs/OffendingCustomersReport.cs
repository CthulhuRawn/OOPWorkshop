using System.ComponentModel.DataAnnotations;

namespace EZVet.DTOs
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
