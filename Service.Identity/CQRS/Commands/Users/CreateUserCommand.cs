using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Service.Base.ViewModels.Common;
using Service.Identity.Contracts;
using Service.Identity.Models;
using Service.Identity.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Commands.Users
{
    public class CreateUserCommand:IRequest<SuccessResponseVM>
    {
        public CreateUserVM Payload { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, SuccessResponseVM>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        public CreateUserCommandHandler(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<SuccessResponseVM> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();

            var request = command.Payload.User;
            var actor = command.Payload.Actor.Name;

            using (var user = _userRepository.CreateTransaction((int)IsolationLevel.Serializable))
            {
                try
                {
                    User newUser = new User()
                    {
                        FirstName = request.Firstname,
                        LastName = request.Lastname,
                        Email = request.Email,
                        Phone = request.Phone,
                        UserTypeId = request.UserTypeId,
                        GroupId = request.GroupId
                    };
                    newUser.SetPassword(request.Password, _passwordHasher);

                    _userRepository.SetActor(actor);
                    _userRoleRepository.SetActor(actor);

                    var userCreated = await _userRepository.CreateAsync(newUser);

                    UserRole newUserRole = new UserRole()
                    {
                        UserId = userCreated.Id,
                        RoleId = request.RoleId
                    };

                    await _userRoleRepository.CreateAsync(newUserRole);

                    result.IsSuccess = true;

                    await _userRepository.CommitTransaction(user);

                }
                catch (Exception ex)
                {
                    await _userRepository.RollbackTransaction(user);
                    throw ex;
                }
            }
            return result;
        }
    }
}
