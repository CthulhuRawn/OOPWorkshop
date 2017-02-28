using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace EZVet.Common
{
    public class DateTimeParameterBinding : HttpParameterBinding
    {
        public DateTimeParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {
        }

        public override Task ExecuteBindingAsync(
            ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext,
            CancellationToken cancellationToken)
        {
            string dateToParse = null;
            string paramName = this.Descriptor.ParameterName;


            // reading from query string
            var nameVal = actionContext.Request.GetQueryNameValuePairs();
            dateToParse =
                nameVal.Where(q => string.Equals(q.Key, paramName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault().Value;


            DateTime? dateTime = null;
            if (!string.IsNullOrEmpty(dateToParse))
            {
                dateTime = ParseDateTime(dateToParse);
            }

            SetValue(actionContext, dateTime);

            return Task.FromResult<object>(null);
        }

        public DateTime? ParseDateTime(
            string dateToParse)
        {
            DateTime validDate;

            if (DateTime.TryParseExact(dateToParse, "dd/MM/yyyy", null, DateTimeStyles.AssumeLocal, out validDate))
            {
                return validDate;
            }

            return null;
        }
    }
}