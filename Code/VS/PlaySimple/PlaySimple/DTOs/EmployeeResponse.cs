using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.DTOs
{
    public class EmployeeResponse
    {
        public bool AlreadyExists { get; set; }

        public Employee Employee { get; set; }
    }
}