using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Data.ViewModels.VoucherType
{
    public class CreateVoucherTypeRequestVM
    {
        public VoucherTypeVM VoucherType { get; set; }
    }

    public class VoucherTypeVM
    {
        public string Name { get; set; }
    }

    public class VoucherTypeResponseVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
