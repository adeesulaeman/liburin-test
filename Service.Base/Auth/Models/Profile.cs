using Service.Base.Auth.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Auth.Models
{
    public class Profile : IProfile
    {
        public string Name { get; set; }
        public List<long> RoleIDs { get; set; }
        public long CompanyID { get; set; }
        public string CompanyName { get; set; }
        public long UserID { get; set; }
        public string Email { get; set; }
        public long GroupID { get; set; }
        public string IPAddress { get; set; }

        public Profile() { }

        public Profile(UserAuthProfile authProfile)
        {
            Name = authProfile.Name;
            RoleIDs = authProfile.RoleIDs;
            CompanyID = authProfile.CompanyID;
            CompanyName = authProfile.CompanyName;
            UserID = authProfile.UserID;
            Email = authProfile.Email;
            GroupID = authProfile.GroupID;
            IPAddress = authProfile.IPAddress;
        }
    }

    public class ActorProfile : IActorProfile
    {
        public string Name { get; set; }
        public string IpAddress { get; set; }

        public ActorProfile(string actorName, string ipAddress)
        {
            Name = actorName;
            IpAddress = ipAddress;
        }
    }
}
