using Service.Base.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Identity.ViewModels.User
{
    public class CreateUserVM
    {
        public UserRequestVM User { get; set; }
        public ProfileVM Actor { get; set; }
    }

    public class UserRequestVM
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long UserTypeId { get; set; }
        public long GroupId { get; set; }
        public long RoleId { get; set; }
        public string Password { get; set; }
    }

    public class UsersResponseVM
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public long UserTypeId { get; set; }
        public long GroupId { get; set; }
        public string GroupName { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
    }

}
