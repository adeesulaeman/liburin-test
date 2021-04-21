using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Auth.Models
{
    public class UserAuthProfile : Profile
    {
        public string Phone { get; set; }
        public ICollection<UserAuthPermission> Permissions { get; set; }

        public UserAuthProfile()
        {
            Permissions = new HashSet<UserAuthPermission>();
        }
    }
}
