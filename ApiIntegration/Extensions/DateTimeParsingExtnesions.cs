namespace ApiIntegration.Extensions
{
    using System;
    using System.Globalization;
    using System.IO;

    public static class DateTimeParsingExtensions
    {
        public static DateTime ToDateTime(this string value)
        {
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var dateValue))
            {
                return dateValue;
            }

            throw new InvalidDataException($"Format of the date was invalid: {value}");
        }
    }
}