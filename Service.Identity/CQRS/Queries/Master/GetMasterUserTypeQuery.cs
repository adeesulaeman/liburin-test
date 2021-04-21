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
    public class GetMasterUserTypeQuery : IRequest<List<UserType>>
    {

    }

    public class GetMasterUserTypeQueryHandler : IRequestHandler<GetMasterUserTypeQuery, List<UserType>>
    {
        private readonly IUserTypeRepository _iUserTypeRepository;
        public GetMasterUserTypeQueryHandler(IUserTypeRepository iUserTypeRepository)
        {
            _iUserTypeRepository = iUserTypeRepository;
        }
        public async Task<List<UserType>> Handle(GetMasterUserTypeQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<UserType, bool>> predicate = p => p.IsActive;

            var rawData = await _iUserTypeRepository.GetEntitiesAsync(predicate);

            var resData = rawData.ToList();

            return resData;
        }
    }
}
