using Service.Base.Repository;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Repositories
{
    public class OperationRepository:BaseReadOnlyRepository<Operation>, IOperationRepository
    {
        public OperationRepository(IdentityContext context) : base(context) { }

        //public override Operation OnCreating(Operation entity) => entity;

        //public override Operation OnUpdating(Operation local, Operation db) => db;
    }
}
