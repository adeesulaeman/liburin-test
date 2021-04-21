using MediatR;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Queries.Master
{
    public class GetMasterRoleQuery : IRequest<List<Role>>
    {
    }

    public class GetMasterRoleQueryHandler : IRequestHandler<GetMasterRoleQuery, List<Role>>
    {
        private readonly IRoleRepository _iRoleRepository;
        public GetMasterRoleQueryHandler(IRoleRepository iRoleRepository)
        {
            _iRoleRepository = iRoleRepository;
        }
        public async Task<List<Role>> Handle(GetMasterRoleQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Role, bool>> predicate = p => p.IsActive;

            var rawData = await _iRoleRepository.GetEntitiesAsync(predicate);

            var resData = rawData.ToList();

            return resData;
        }
    }
}
