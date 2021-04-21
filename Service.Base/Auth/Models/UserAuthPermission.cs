using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Auth.Models
{
    public class UserAuthPermission
    {
        public string Module { get; set; }
        public List<string> Operations { get; set; }

        public UserAuthPermission()
        {
            Operations = new List<string>();
        }
    }
}
