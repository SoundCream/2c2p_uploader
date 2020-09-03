using System;
using System.Globalization;

namespace _2C2P.FileUploader.Helper
{
    public class GlobalHelper
    {
        public static bool IsDecimal(string number)
        {
            var result = decimal.TryParse(number, out decimal value);
            return result;
        }

        public static bool IsDecimalNumber(string number)
        {
            var result = decimal.TryParse(number.Replace(",", string.Empty).Replace(".", string.Empty), NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value);
            return result;
        }

        public static decimal ParseDecimalNumber(string number)
        {
            var result = default(decimal);
            decimal.TryParse(number.Replace(",", string.Empty), NumberStyles.Number, CultureInfo.InvariantCulture, out result);
            return result;
        }

        public static bool IsCorrectDateTimeFormat(string dateTime, string format)
        {
            var result = DateTime.TryParseExact(dateTime, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value);
            return result;
        }

        public static DateTime GetDateTimeFromString(string dateTime, string format)
        {
            var result = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
            return result;
        }
    }
}
