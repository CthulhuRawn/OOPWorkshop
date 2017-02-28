using System;

namespace EZVet.DTOs
{
    public class FinanceReport
    {
        public virtual string Date{ get; set; }

        public virtual int NumOfVisits{ get; set; }

        public virtual double Price { get; set; }
    }
}