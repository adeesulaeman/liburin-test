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
    public class GetMasterGroupUserQuery : IRequest<List<Group>>
    {
    }

    public class GetMasterGroupUserQueryHandler : IRequestHandler<GetMasterGroupUserQuery, List<Group>>
    {
        private readonly IGroupRepository _iGroupRepository;
        public GetMasterGroupUserQueryHandler(IGroupRepository iGroupRepository)
        {
            _iGroupRepository = iGroupRepository;
        }
        public async Task<List<Group>> Handle(GetMasterGroupUserQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Group, bool>> predicate = p => p.IsActive;

            var rawData = await _iGroupRepository.GetEntitiesAsync(predicate);

            var resData = rawData.ToList();

            return resData;
        }
    }
}
