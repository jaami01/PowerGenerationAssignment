using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerGeneration.Common
{
    public static class StringExtensions
    {
        public static decimal ValidOrDefaultDecimal(this string value)
        {
            decimal tryParseValue;
            if (string.IsNullOrWhiteSpace(value)) return 0;
            bool valid = decimal.TryParse(value, out tryParseValue);
            if (valid) return tryParseValue;
            else return 0;
        }
        public static DateTime ValidOrDefaultDate(this string value)
        {
            DateTime tryParseValue;
            if (string.IsNullOrWhiteSpace(value)) return DateTime.MinValue;
            bool valid = DateTime.TryParse(value, out tryParseValue);
            if (valid) return tryParseValue;
            else return DateTime.MinValue;
        }
    }
}
