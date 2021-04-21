using Microsoft.EntityFrameworkCore;
using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class RoleRepository:BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IdentityContext context) : base(context) { }

        public override Role OnUpdating(Role local, Role db) => db;

        public override Role OnCreating(Role entity) => entity;

        public override Task<Role> GetWithRelationsAsync(long id)
        {
            return Task.Run(
                () => Context.Set<Role>()
                               .Include(x => x.Group)
                               .Include(x => x.Permissions).ThenInclude(y => y.Module)
                               .Include(x => x.Permissions).ThenInclude(y => y.Operation)
                               .SingleOrDefaultAsync(x => x.Id == id)
                );
        }

        public override Task<IEnumerable<Role>> GetWithRelationsAsync(Expression<Func<Role, bool>> searchPredicate)
        {
            return Task.Run(
                () => Context.Set<Role>()
                               .Where(searchPredicate)
                               .Include(x => x.Group)
                               .Include(x => x.Permissions).ThenInclude(y => y.Module)
                               .Include(x => x.Permissions).ThenInclude(y => y.Operation)
                               .OrderBy(r => r.Id)
                               .AsEnumerable()
                );
        }
    }
}
