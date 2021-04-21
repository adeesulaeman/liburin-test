using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class ModuleRepository:BaseReadOnlyRepository<Module>, IModuleRepository
    {
        public ModuleRepository(IdentityContext context) : base(context) { }

        //public override Module OnCreating(Module entity) => entity;

        //public override Module OnUpdating(Module local, Module db) => db;
    }
}
