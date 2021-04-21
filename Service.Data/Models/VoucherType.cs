using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data.Models
{
    public class VoucherType : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public ICollection<Voucher> Voucher { get; set; }
        public VoucherType()
        {
            Voucher = new HashSet<Voucher>();
        }
    }
}
