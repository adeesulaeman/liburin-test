using Service.Base.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Contracts
{
    public interface IUserRepository : IRepository<User> { }
    public interface IUserRoleRepository : IRepository<UserRole> { }
    public interface IUserTypeRepository : IRepository<UserType> { }
    public interface IRoleRepository : IRepository<Role> { }
    public interface IPermissionRepository : IRepository<Permission> { }
    public interface IGroupRepository : IRepository<Group> { }
    public interface IOperationRepository : IReadOnlyRepository<Operation> { }
    public interface IModuleRepository : IReadOnlyRepository<Module> { }
}
