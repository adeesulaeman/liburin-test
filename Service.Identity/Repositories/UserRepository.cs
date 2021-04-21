using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IdentityContext context) : base(context) { }

        public override User OnCreating(User entity) => entity;

        public override User OnUpdating(User local, User db) => db;
    }
}
