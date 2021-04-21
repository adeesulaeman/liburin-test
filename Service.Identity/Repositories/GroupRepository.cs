using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class GroupRepository:BaseRepository<Group>, IGroupRepository
    {
        public GroupRepository(IdentityContext context) : base(context) { }

        public override Group OnCreating(Group entity) => entity;

        public override Group OnUpdating(Group local, Group db) => db;
    }
}
