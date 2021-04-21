using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data.Models
{
    public class Voucher : BaseEntity, IEntity
    {
        public int Value { get; set; }
        public DateTime PeriodFrom { get; set; }
        public DateTime PeriodUntil { get; set; }
        public string VoucherCode { get; set; }
        public string Name { get; set; }
        public int QtyVoucher { get; set; }
        public int MaxQtyPerUser { get; set; }
        public int MaxQtyPerUserPerDay { get; set; }
        public int MinOrderAmount { get; set; }
        public int MaxDiscount { get; set; }
        public int LimitPerDay { get; set; }
        public int IsPublic { get; set; }
        public ICollection<Promo> Promo { get; set; }
        [ForeignKey("discountType_id")]
        public long DiscountTypeId { get; set; }
        public DiscountType DiscountType { get; set; }
        [ForeignKey("voucherType_id")]
        public long VoucherTypeId { get; set; }
        public VoucherType VoucherType { get; set; }
        [ForeignKey("discountInputType_id")]
        public long DiscountInputTypeId { get; set; }
        public DiscountInputType DiscountInputType { get; set; }

        public Voucher()
        {
            Promo = new HashSet<Promo>();
        }
    }
}
