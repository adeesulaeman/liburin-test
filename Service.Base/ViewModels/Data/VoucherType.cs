using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.ViewModels.Data
{
    class VoucherType
    {
    }

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
