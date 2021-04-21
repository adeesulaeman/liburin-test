using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.ViewModels.Identity
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
        public long Id { get; set; }
        public string Name { get; set; }
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

        public PermissionByRoleResponseVM()
        {
            ModuleName = string.Empty;
        }
    }

    public class RoleResponseVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
    }
}
