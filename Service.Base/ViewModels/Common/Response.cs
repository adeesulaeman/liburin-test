using Service.Base.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.ViewModels.Common
{
    public class SuccessResponseVM : IResponse
    {
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }

        public SuccessResponseVM()
        {
            Reason = string.Empty;
        }
    }
}
