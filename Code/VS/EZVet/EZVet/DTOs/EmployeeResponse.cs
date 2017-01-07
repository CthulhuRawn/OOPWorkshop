using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EZVet.DTOs
{
    public class EmployeeResponse
    {
        public bool AlreadyExists { get; set; }

        public Employee Employee { get; set; }
    }
}