using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.DTOs
{
    public class UsingFieldsReport
    {
        public virtual int FieldId { get; set; }

        public virtual string FieldName { get; set; }

        public virtual int WeekDayOrders { get; set; }

        public virtual int WeekEndOrders { get; set; }

        public virtual int hours16_18Orders { get; set; }

        public virtual int hours18_20Orders { get; set; }

        public virtual int hours20_22Orders { get; set; }
    }
}