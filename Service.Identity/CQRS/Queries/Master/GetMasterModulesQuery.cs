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
    public class GetMasterModulesQuery : IRequest<List<Module>>
    {
    }

    public class GetModulesQueryHandler : IRequestHandler<GetMasterModulesQuery, List<Module>>
    {
        private readonly IModuleRepository _moduleRepository;
        public GetModulesQueryHandler(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }
        public async Task<List<Module>> Handle(GetMasterModulesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Module, bool>> predicate = p => p.IsActive;

            var rawData = await _moduleRepository.GetEntitiesAsync(predicate);

            var resData = rawData.ToList();

            return resData;
        }
    }
}
