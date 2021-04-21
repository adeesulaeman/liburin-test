using Service.Base.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.ViewModels.Role
{
    public class CreateRoleVM
    {
        public string Name { get; set; }
        public long GroupId { get; set; }

        public IEnumerable<PermissionByRoleVM> Permissions { get; set; }

        public CreateRoleVM()
        {
            Permissions = new HashSet<PermissionByRoleVM>();
        }
    }

    public class CreateRoleRequestVM
    {
        public RoleVM Role { get; set; }
        public ProfileVM Actor { get; set; }
    }
    public class PermissionByRoleVM
    {
        public long ModuleId { get; set; }
        public ICollection<long> OperationId { get; set; }
    }

    public class RoleVM
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public long GroupId { get; set; }

        public IEnumerable<PermissionByRoleVM> Permissions { get; set; }

        public RoleVM()
        {
            Permissions = new HashSet<PermissionByRoleVM>();
        }
    }

    //response view model
    public class PermissionByRoleResponseVM
    {
        public long Id { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public long OperationId { get; set; }
    }

    public class RoleResponseVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
    }
}
