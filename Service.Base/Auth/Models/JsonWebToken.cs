using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Auth.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}
