using Service.Base.Auth.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Base.Auth.Models
{
    public class UserHubProfile : Profile
    {
        public string ConnectionID { get; set; }
        public string Room { get; set; }

        public UserHubProfile(IProfile profile)
        {
            Name = profile.Name;
            RoleIDs = profile.RoleIDs;
            UserID = profile.UserID;
            Email = profile.Email;
            GroupID = profile.GroupID;

            Room = $"{profile.GroupID}";
        }
    }
}
