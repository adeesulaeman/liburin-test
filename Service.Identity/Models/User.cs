using Microsoft.AspNetCore.Identity;
using Service.Base.Contracts;
using Service.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models
{
    public class User:BaseEntity,IEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }

        [ForeignKey("user_type_id")]
        public long UserTypeId { get; set; }
        public UserType UserType { get; set; }

        [ForeignKey("group_id")]
        public long GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public void SetPassword(string password, IPasswordHasher<User> passwordHasher)
        {
            Password = passwordHasher.HashPassword(this, password);
        }
        public bool ValidatePassword(string password, IPasswordHasher<User> passwordHasher)
        {
            var passwordHash = passwordHasher.HashPassword(this, password);
            return passwordHasher.VerifyHashedPassword(this, Password, password) != PasswordVerificationResult.Failed;
        }
    }
}
