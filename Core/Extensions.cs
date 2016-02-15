using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaxReportGenerator.Core
{
    public static class Extensions
    {
        public static T ToObject<T>(this string input)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(input);
        }

        public static bool MatchesRegex(this string input, string regexFormat)
        {
            return Regex.IsMatch(input, regexFormat);
        }
    }
}
