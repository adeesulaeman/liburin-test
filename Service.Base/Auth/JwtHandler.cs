using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

using Service.Base.Auth.Contracts;
using Service.Base.Auth.Models;
using Service.Base.Helper;

namespace Service.Base.Auth
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtOptions _options;
        public JwtHandler(JwtOptions options)
        {
            _options = options;
        }
        public JsonWebToken CreateToken(IEnumerable<Claim> claims)
        {
            var expires = DateTime.Now.AddMinutes(_options.Expires);
            var jwt = new JwtSecurityToken(
                issuer: _options.ValidIssuer,
                audience: _options.ValidAudience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: _options.SigningCredentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                RefreshToken = string.Empty,
                Expires = expires.ToTimestamp()
            };
        }
        public UserHubProfile GetUserProfile(HubCallerContext context)
        {
            var user = context.User;
            var claimsIdentity = user.Identity as ClaimsIdentity;

            IProfile profile = GetUserProfile(claimsIdentity);

            var userHubProfile = new UserHubProfile(profile);
            userHubProfile.ConnectionID = context.ConnectionId;

            return userHubProfile;
        }

        public UserAuthProfile GetUserProfile(IHttpContextAccessor accessor)
        {
            var result = GetUserProfile(accessor.HttpContext);
            result.IPAddress = IpHelper.GetRequestIP(accessor);

            return result;
        }

        public UserAuthProfile GetUserProfile(HttpContext context)
        {
            var user = context.User;
            var claimsIdentity = user.Identity as ClaimsIdentity;

            return GetUserProfile(claimsIdentity);
        }

        public UserAuthProfile GetUserProfile(ClaimsIdentity identity)
        {
            var profile = new UserAuthProfile();

            var claimName = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var claimRole = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            var claimPermission = identity.Claims.FirstOrDefault(x => x.Type == "permission");
            var claimUserID = identity.Claims.FirstOrDefault(x => x.Type == "userID");
            var claimGroupID = identity.Claims.FirstOrDefault(x => x.Type == "groupID");
            var claimEmail = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
            var claimPhone = identity.Claims.FirstOrDefault(x => x.Type == "phone");

            profile.Name = claimName != null ? claimName.Value : string.Empty;
            profile.RoleIDs = claimRole != null ? JsonConvert.DeserializeObject<List<long>>(claimRole.Value) : new List<long>();
            profile.Permissions = claimPermission != null
                                    ? JsonConvert.DeserializeObject<ICollection<UserAuthPermission>>(claimPermission.Value)
                                    : new HashSet<UserAuthPermission>();
            profile.UserID = claimUserID != null ? Convert.ToInt64(claimUserID.Value) : 0;
            profile.GroupID = claimGroupID != null ? Convert.ToInt64(claimGroupID.Value) : 0;
            profile.Email = claimEmail != null ? claimEmail.Value : "";
            profile.Phone = claimPhone != null ? claimPhone.Value : "";

            return profile;
        }
    }
}
