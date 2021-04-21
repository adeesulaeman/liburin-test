using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IdentityContext context) : base(context) { }

        public override UserRole OnCreating(UserRole entity) => entity;

        public override UserRole OnUpdating(UserRole local, UserRole db) => db;
    }
}
