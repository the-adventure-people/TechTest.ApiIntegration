using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiIntegration.Logging
{
    public static class Helpers
    {
        public const string RequestTitle = "REQUEST: ";
        public const string ResponseTitle = "RESPONSE: ";
        public const string DurationTitle = "DURATION: ";

        public static string Duration(DateTime startTime)
        {
            return $"{DurationTitle}{DateTime.Now.Subtract(startTime).TotalMilliseconds}ms{NewLines()}";
        }

        public static string FormatLogInfo(string title, string information)
        {
            return $"{title}{information}{NewLines()}";
        }

        private static IEnumerable<string> NewLines()
        {
            return Enumerable.Repeat(Environment.NewLine, 2);
        }
    }
}