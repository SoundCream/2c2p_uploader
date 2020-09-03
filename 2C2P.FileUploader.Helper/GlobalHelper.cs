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
