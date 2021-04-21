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
    public class UpdateRoleCommand:IRequest<SuccessResponseVM>
    {
        public RoleVM Payload { get; set; }
        public long Id { get; set; }
        public ProfileVM Actor { get; set; }
    }

    public class UpdateRoleCommandHandler:IRequestHandler<UpdateRoleCommand, SuccessResponseVM>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }
        public async Task<SuccessResponseVM> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();

            var request = command.Payload;
            var actor = command.Actor.Name;
            var roleId = command.Id;

            using (var role = _roleRepository.CreateTransaction((int)IsolationLevel.Serializable))
            {
                try
                {
                    var existRole = await _roleRepository.GetWithRelationsAsync(roleId);
                    if (existRole == null)
                        throw new Exception("Ups.. Role not exist");

                    var updatePermission = existRole.Permissions;

                    existRole.Name = (request.Name ?? "") != "" ? request.Name : existRole.Name;
                    existRole.GroupId = request.GroupId != 0 ? request.GroupId : existRole.GroupId;

                    _roleRepository.SetActor(actor);
                    await _roleRepository.UpdateAsync(existRole);

                    List<Permission> requestPermission = new List<Permission>();

                    foreach (var module in request.Permissions)
                    {
                        foreach (var operation in module.OperationId)
                        {
                            Permission permission = new Permission
                            {
                                RoleId = roleId,
                                ModuleId = module.ModuleId,
                                OperationId = operation
                            };
                            requestPermission.Add(permission);
                        }
                    }

                    var toRemovePermission = existRole.Permissions.Where(x => !requestPermission.Where(y => y.ModuleId == x.ModuleId)
                                                                                                .Select(y => y.OperationId)
                                                                                                .Contains(x.OperationId)
                                                                         ).ToList();
                    var toAddPermission = requestPermission.Where(x => !existRole.Permissions.Where(y => y.ModuleId == x.ModuleId)
                                                                                            .Select(y => y.OperationId)
                                                                                            .Contains(x.OperationId)
                                                                  ).ToList();
                    foreach(var permission in toAddPermission)
                    {
                        permission.RoleId = roleId;
                        _permissionRepository.SetActor(actor);
                        await _permissionRepository.CreateAsync(permission);
                    }

                    foreach(var permission in toRemovePermission)
                    {
                        _permissionRepository.SetActor(actor);
                        await _permissionRepository.DeleteAsync(permission.Id);
                    }

                    result.IsSuccess = true;

                    await _roleRepository.CommitTransaction(role);
                }
                catch (Exception ex)
                {
                    await _roleRepository.RollbackTransaction(role);
                    throw ex;
                }
            }

            return result;
        }
    }
}
