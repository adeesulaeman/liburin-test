using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Service.Base.Auth.Contracts;
using Service.Base.Auth.Models;
using Service.Identity.Contracts;
using Service.Identity.Models;
using Service.Identity.Models.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Queries.Users
{
    public class RefreshTokenQuery:IRequest<JsonWebToken>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenQueryHandler : IRequestHandler<RefreshTokenQuery, JsonWebToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IJwtHandler _jwtHandler;
        private readonly JwtOptions _options;

        public RefreshTokenQueryHandler(
            IPasswordHasher<User> passwordHasher,
            IUserRepository userRepository,
            IDistributedCache cache,
            IClaimsProvider claimsProvider,
            IJwtHandler jwtHandler,
            JwtOptions options)
        {
            _userRepository = userRepository;
            _cache = cache;
            _passwordHasher = passwordHasher;
            _claimsProvider = claimsProvider;
            _jwtHandler = jwtHandler;
            _options = options;
        }

        public async Task<JsonWebToken> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var jsonStringRefreshToken = await _cache.GetStringAsync($"RefreshToken_{request.RefreshToken}");

            if(jsonStringRefreshToken == null || string.IsNullOrEmpty(jsonStringRefreshToken))
                throw new Exception("Token Not found!");

            var cacheRefreshToken = JsonConvert.DeserializeObject<RefreshToken>(jsonStringRefreshToken);

            var user = await _userRepository.GetByIdAsync(cacheRefreshToken.UserId);

            if (user == null)
                throw new Exception("User was not found !");

            var jwtToken = _jwtHandler.CreateToken(await _claimsProvider.GetAsync(user));
            jwtToken.RefreshToken = cacheRefreshToken.Token;

            return jwtToken;
        }
    }
}
