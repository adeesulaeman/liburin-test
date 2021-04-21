using Service.Base.Repository;
using Service.Data.Contracts;
using Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data.Repositories
{
    public class VocTypeRepository : BaseRepository<VoucherType>, IVoucherTypeRepository
    {
        public VocTypeRepository(DataContext context) : base(context) { }

        public override VoucherType OnCreating(VoucherType entity) => entity;

        public override VoucherType OnUpdating(VoucherType local, VoucherType db) => db;
    }
}
