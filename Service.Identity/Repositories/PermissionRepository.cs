using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class PermissionRepository:BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IdentityContext context) : base(context) { }

        public override Permission OnCreating(Permission entity) => entity;

        public override Permission OnUpdating(Permission local, Permission db) => db;
    }
}
