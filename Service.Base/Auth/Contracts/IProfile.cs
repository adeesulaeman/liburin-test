using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Auth.Contracts
{
    public interface IProfile
    {
        string Name { get; set; }
        List<long> RoleIDs { get; set; }
        long UserID { get; set; }
        string Email { get; set; }
        long GroupID { get; set; }
    }

    public interface IActorProfile
    {
        string Name { get; set; }
        string IpAddress { get; set; }
    }
}
