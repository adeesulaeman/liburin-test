using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.ApiLog.Model
{
    public class ApiLogItem
    {
        public long Id { get; set; }

        public DateTime RequestTime { get; set; }

        public long ResponseMillis { get; set; }

        public int StatusCode { get; set; }

        public string Method { get; set; }

        public string Path { get; set; }

        public string QueryString { get; set; }

        public string RequestBody { get; set; }

        public string ResponseBody { get; set; }
    }
}
