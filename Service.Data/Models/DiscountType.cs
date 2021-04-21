using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data.Models
{
    public class DiscountType : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Operation { get; set; }
        public ICollection<Voucher> Voucher { get; set; }
        public DiscountType()
        {
            Voucher = new HashSet<Voucher>();
        }
    }
}
