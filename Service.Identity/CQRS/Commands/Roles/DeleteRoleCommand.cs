using MediatR;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Identity.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Commands.Roles
{
    public class DeleteRoleCommand : IRequest<SuccessResponseVM>
    {
        public long Id { get; set; }
        public ProfileVM Actor { get; set; }
    }

    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, SuccessResponseVM>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;
        public DeleteRoleCommandHandler(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }
        public async Task<SuccessResponseVM> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            var result = new SuccessResponseVM();
            var roleId = command.Id;
            var actor = command.Actor.Name;

            using (var role = _roleRepository.CreateTransaction((int)IsolationLevel.Serializable))
            {
                try
                {
                    _roleRepository.SetActor(actor);
                    await _roleRepository.DeleteAsync(roleId);

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
