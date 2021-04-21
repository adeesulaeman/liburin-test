using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class UserTypeRepository : BaseRepository<UserType>, IUserTypeRepository
    {
        public UserTypeRepository(IdentityContext context) : base(context) { }

        public override UserType OnCreating(UserType entity) => entity;

        public override UserType OnUpdating(UserType local, UserType db) => db;
    }
}
