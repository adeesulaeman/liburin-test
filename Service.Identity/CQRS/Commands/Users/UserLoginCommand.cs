using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Service.Base.Auth.Models;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Commands.Users
{
    public class UserLoginCommand: IRequest<SuccessResponseVM>
    {
        public UserLoginVM Payload { get; set; }
    }

    public class UserLoginQueryHandler : IRequestHandler<UserLoginCommand, SuccessResponseVM>
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IDistributedCache _cache;

        private const int _WAITING_PERIOD_IN_MINUTES = 2;
        private const int _MAXIMUM_LOGIN_TRIES = 15;
        public UserLoginQueryHandler(IPasswordHasher<User> passwordHasher, IUserRepository userRepository, IDistributedCache cache)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _cache = cache;
        }
        public async Task<SuccessResponseVM> Handle(UserLoginCommand command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();

            Expression<Func<User, bool>> predicate = (x) => (x.Email.ToLower() == command.Payload.Username.ToLower() || x.Phone == command.Payload.Username) && x.IsActive;

            var user = (await _userRepository.GetWithRelationsAsync(predicate)).FirstOrDefault();

            var cacheTries = await _cache.GetStringAsync($"login_tries_{command.Payload.Username}");

            int tries = 0;

            if (cacheTries != null)
            {
                tries = Convert.ToInt32(cacheTries);
                await _cache.RemoveAsync($"LoginTries_{command.Payload.Username}");
            }

            tries++;

            if (tries >= _MAXIMUM_LOGIN_TRIES)
                result.Reason = "Illegal login attempts, outside the maximum telorated conditions";
            else if (user == null || !user.ValidatePassword(command.Payload.Password, _passwordHasher))
                result.Reason = "User not exist";
            else if (!user.IsActive)
                result.Reason = "User is't active";

            if(!string.IsNullOrEmpty(result.Reason))
            {
                if(tries < _MAXIMUM_LOGIN_TRIES)
                {
                    var currentTimeUTC = DateTime.UtcNow.ToString();

                    var options = new DistributedCacheEntryOptions();
                    options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_WAITING_PERIOD_IN_MINUTES);

                    byte[] byteTries = BitConverter.GetBytes(tries);
                    await _cache.SetStringAsync($"login_tries_{command.Payload.Username}", tries.ToString(), options);
                }

                result.IsSuccess = false;
            }
            else
            {
                result.IsSuccess = true;
                if(cacheTries != null)
                {
                    await _cache.RemoveAsync($"LoginTries_{command.Payload.Username}");
                }
            }

            return result;
        }
    }
}
