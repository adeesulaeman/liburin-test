using MediatR;
using Service.Base.ViewModels.Identity;
using Service.Identity.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Queries.Roles
{
    public class GetRoleByIdQuery:IRequest<RoleVM>
    {
        public long Id { get; set; }
    }

    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleVM>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleVM> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetWithRelationsAsync(request.Id);

            if (role == null)
                throw new Exception($"Role with id = {request.Id} not exist");
            else
            {
                var result = new RoleVM
                {
                    Id = role.Id,
                    Name = role.Name,
                    GroupId = role.GroupId,
                    Permissions = role.Permissions.GroupBy(x=>new{ x.ModuleId }).Select(x => new PermissionByRoleVM
                    {
                        ModuleId = x.Key.ModuleId,
                        OperationId = x.Select(y=>y.OperationId).ToList()
                    })
                };

                return result;
            }
        }
    }
}
