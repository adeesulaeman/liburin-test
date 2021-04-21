using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.CMS.ViewModels.Identity
{
    public class CreateRole
    {
        public string Name { get; set; }
        public long GroupId { get; set; }

        public IEnumerable<PermissionByRole> Permissions { get; set; }

        public CreateRole()
        {
            Permissions = new HashSet<PermissionByRole>();
        }
    }
    public class PermissionByRole
    {
        public long ModuleId { get; set; }
        public ICollection<long> OperationId { get; set; }
    }
}
