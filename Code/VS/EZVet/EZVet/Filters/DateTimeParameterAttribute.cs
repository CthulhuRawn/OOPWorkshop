using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EZVet.Common;

namespace EZVet.Filters
{
    public class DateTimeParameterAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(
            HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(DateTime?))
            {
                var binding = new DateTimeParameterBinding(parameter);
                
                return binding;
            }

            return parameter.BindAsError("Expected type DateTime?");
        }
    }
}