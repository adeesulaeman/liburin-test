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
    public class GetMasterOperationsQuery : IRequest<List<Operation>>
    {
    }

    public class GetMasterOperationsHandler : IRequestHandler<GetMasterOperationsQuery, List<Operation>>
    {
        private readonly IOperationRepository _operationRepository;
        public GetMasterOperationsHandler(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }
        public async Task<List<Operation>> Handle(GetMasterOperationsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Operation, bool>> predicate = p => p.IsActive;

            var rawData = await _operationRepository.GetEntitiesAsync(predicate);

            var resData = rawData.ToList();

            return resData;
        }
    }
}
