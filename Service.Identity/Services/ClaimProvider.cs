using Newtonsoft.Json;
using Service.Identity.Contracts;
using Service.Identity.Models;
using Service.Identity.Models.JWT;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Service.Identity.Services
{
    public class ClaimProvider : IClaimsProvider
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IOperationRepository _operationRepository;
        private IEnumerable<Module> _modules { get; set; }
        private IEnumerable<Operation> _operations { get; set; }

        public ClaimProvider(IRoleRepository roleRepository, IModuleRepository moduleRepository, IOperationRepository operationRepository)
        {
            _roleRepository = roleRepository;
            _moduleRepository = moduleRepository;
            _operationRepository = operationRepository;

            GetMasterData().Wait();
        }

        private async Task GetMasterData()
        {
            _modules = await _moduleRepository.GetEntitiesAsync();
            _operations = await _operationRepository.GetEntitiesAsync();
        }
        public async Task<IEnumerable<Claim>> GetAsync(User user)
        {
            var claims = new List<Claim>();
            var permissionClaims = new List<PermissionsClaim>();

            List<long> roleIdList = user.UserRoles.Select(s => s.RoleId).ToList();

            foreach(var userRole in user.UserRoles)
            {
                var dbUserRole = await _roleRepository.GetByIdAsync(userRole.RoleId);

                if(dbUserRole != null)
                {
                    foreach(var moduleId in dbUserRole.Permissions.Select(x=>x.ModuleId).Distinct())
                    {
                        var permissionClaim = new PermissionsClaim();

                        permissionClaim.Module = _modules.SingleOrDefault(x => x.Id == moduleId).Value;
                        permissionClaim.Permission = _operations.Where(y => userRole.Role.Permissions.Where(x => x.ModuleId == moduleId)
                                                                                                    .Select(x => x.OperationId)
                                                                                                    .ToList()
                                                                                                    .Contains(y.Id))
                                                                  .Select(x => x.Value)
                                                                  .ToList();
                        permissionClaims.Add(permissionClaim);
                    }
                }
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, $"{user.FirstName}"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, string.IsNullOrEmpty(user.FirstName) ? "" : user.FirstName.ToLower()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, (string.IsNullOrEmpty(user.Email) ? "" : user.Email.ToLower())));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));
            claims.Add(new Claim("role", JsonConvert.SerializeObject(roleIdList)));

            return claims;
        }
    }
}
