using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySimple.Common
{
    // TODO add to doc
    public class DateUtils
    {
        public static int GetAge(DateTime dateTime)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateTime.Year;

            if (dateTime > today.AddYears(-age))
                age--;

            return age;
        }

        public static DateTime GetXYearsEarly(int age)
        {
            DateTime today = DateTime.Today;
            DateTime past = today.AddYears(-age);
            return past;
        }

        public static IList<DateTime> PossibleDateOrders(DateTime dateTime)
        {
            IList<DateTime> possibleHour = new List<DateTime>()
            {
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 16, 0, 0, DateTimeKind.Local),
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 18, 0, 0, DateTimeKind.Local),
                new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 20, 0, 0, DateTimeKind.Local),
            };

            return possibleHour;
        }

        private static DateTime _jan1st1970 = new DateTime(1970, 1, 1);

        /// <summary>
        /// Converts a DateTime into a (JavaScript parsable) Int64.
        /// </summary>
        /// <param name="from">The DateTime to convert from</param>
        /// <returns>An integer value representing the number of milliseconds since 1 January 1970 00:00:00 UTC.</returns>
        public static long ConvertToJavaScript(DateTime from)
        {
            return System.Convert.ToInt64((from - _jan1st1970).TotalMilliseconds);
        }

        /// <summary>
        /// Converts a (JavaScript parsable) Int64 into a DateTime.
        /// </summary>
        /// <param name="from">An integer value representing the number of milliseconds since 1 January 1970 00:00:00 UTC.</param>
        /// <returns>The date as a DateTime</returns>
        public static DateTime ConvertFromJavaScript(long from)
        {
            return _jan1st1970.AddMilliseconds(from);
        }
    }
}