using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Identity.Models.JWT
{
    public class RefreshToken
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public RefreshToken() { }
        public RefreshToken(User user, IPasswordHasher<User> passwordHasher)
        {
            UserId = user.Id;
            CreatedAt = DateTime.Now;
            Token = CreateToken(user, passwordHasher);
        }

        private static string CreateToken(User user, IPasswordHasher<User> passwordHasher)
        {
            return passwordHasher.HashPassword(user, Guid.NewGuid().ToString("N"))
                .Replace("=", string.Empty)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty);
        }
    }
}
