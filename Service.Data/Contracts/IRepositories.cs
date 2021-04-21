using Service.Base.Contracts;
using Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data.Contracts
{
    public interface IVoucherTypeRepository : IRepository<VoucherType> { }
    public interface IPromoRepository : IRepository<Promo> { }
}
