using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Base.Helper
{
    public static class IpHelper
    {
        public static string GetRequestIP(IHttpContextAccessor accessor)
        {
            string ip = "";

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            ip = GetHeaderValueAs<string>("X-Forwarded-For", accessor).SplitCsv().FirstOrDefault();

            
            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("X-Real-IP", accessor);

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>("REMOTE_ADDR", accessor);

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && accessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = accessor.HttpContext.Connection.RemoteIpAddress.ToString();

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            //if (ip.IsNullOrWhitespace())
            //    throw new Exception("Unable to determine caller's IP.");

            return ip;
        }

        private static T GetHeaderValueAs<T>(string headerName, IHttpContextAccessor accessor)
        {
            StringValues values;

            if (accessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }

        private static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }

        private static bool IsNullOrWhitespace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }
    }
}
