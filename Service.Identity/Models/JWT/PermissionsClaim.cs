using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models.JWT
{
    public class PermissionsClaim
    {
        public string Module { get; set; }
        public IEnumerable<string> Permission { get; set; }

        public PermissionsClaim()
        {
            Permission = new HashSet<string>();
        }
    }
}
