using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Service.Base.Auth.Contracts;
using Service.Base.Auth.Models;
using Service.Base.ViewModels.Identity;
using Service.Identity.Contracts;
using Service.Identity.Models;
using Service.Identity.Models.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Queries.Users
{
    public class GetTokenQuery:IRequest<JsonWebToken>
    {
        public UserLoginVM Payload { get; set; }
    }

    public class GetTokenQueryHandler : IRequestHandler<GetTokenQuery, JsonWebToken>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IJwtHandler _jwtHandler;
        private readonly JwtOptions _options;

        public GetTokenQueryHandler(
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
        public async Task<JsonWebToken> Handle(GetTokenQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> predicate = (x) => (x.Email.ToLower() == request.Payload.Username.ToLower() || x.Phone == request.Payload.Username) && x.IsActive;

            var user = (await _userRepository.GetWithRelationsAsync(predicate)).FirstOrDefault();

            if (user == null || !user.ValidatePassword(request.Payload.Password, _passwordHasher))
                throw new Exception("User Not found!");

            RefreshToken refreshToken = new RefreshToken(user, _passwordHasher);
            var claims = await _claimsProvider.GetAsync(user);
            var jwtToken = _jwtHandler.CreateToken(claims);

            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.RefreshTokenExpires);

            await _cache.SetStringAsync($"RefreshToken_{refreshToken.Token}", JsonConvert.SerializeObject(refreshToken), options);

            JsonWebToken response = new JsonWebToken()
            {
                AccessToken = jwtToken.AccessToken,
                Expires = jwtToken.Expires,
                RefreshToken = refreshToken.Token
            };

            return response;
        }
    }
}
