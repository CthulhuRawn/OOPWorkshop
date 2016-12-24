using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.DTOs
{
    public class CustomerUpdateResponse
    {
        public Customer Customer { get; set; }

        public string AuthenticationKey { get; set; }
    }
}