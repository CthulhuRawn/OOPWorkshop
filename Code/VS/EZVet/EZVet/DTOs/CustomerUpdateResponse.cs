using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZVet.DTOs
{
    public class CustomerUpdateResponse
    {
        public Customer Customer { get; set; }

        public string AuthenticationKey { get; set; }
    }
}