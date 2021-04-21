using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.ViewModels.Identity
{
    public class ProfileVM
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }

        public ProfileVM()
        {
            Name = string.Empty;
            IpAddress = string.Empty;
        }
    }
}
