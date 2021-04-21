using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Service.Base.Auth.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Service.Base.Auth.Contracts
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(IEnumerable<Claim> claims);
        UserAuthProfile GetUserProfile(IHttpContextAccessor accessor);
        UserAuthProfile GetUserProfile(HttpContext context);
        UserHubProfile GetUserProfile(HubCallerContext context);
    }
}
