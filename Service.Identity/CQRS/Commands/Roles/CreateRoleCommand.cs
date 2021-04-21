using MediatR;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Commands.Roles
{
    public class CreateRoleCommand:IRequest<SuccessResponseVM>
    {
        public CreateRoleRequestVM Payload { get; set; }
    }

    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, SuccessResponseVM>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public CreateRoleHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<SuccessResponseVM> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();

            var request = command.Payload.Role;
            var actor = command.Payload.Actor.Name;

            using(var role = _roleRepository.CreateTransaction((int)IsolationLevel.Serializable))
            {
                try
                {
                    Role data = new Role
                    {
                        Name = request.Name,
                        Value = request.Name.ToLower().Replace(" ","_"),
                        GroupId = request.GroupId
                    };
                    _roleRepository.SetActor(actor);

                    var roleCreated = await _roleRepository.CreateAsync(data);

                    foreach (var module in request.Permissions)
                    {
                        foreach (var operation in module.OperationId)
                        {
                            Permission permission = new Permission
                            {
                                RoleId = roleCreated.Id,
                                ModuleId = module.ModuleId,
                                OperationId = operation
                            };

                            _permissionRepository.SetActor(actor);

                            await _permissionRepository.CreateAsync(permission);
                        }
                    }

                    result.IsSuccess = true;

                    await _roleRepository.CommitTransaction(role);
                }
                catch(Exception ex)
                {
                    await _roleRepository.RollbackTransaction(role);
                    throw ex;
                }
            }

            return result;
        }
    }
}
