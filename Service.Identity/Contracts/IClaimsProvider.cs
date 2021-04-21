using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service.Identity.Contracts
{
    public interface IClaimsProvider
    {
        Task<IEnumerable<Claim>> GetAsync(User user);
    }
}
