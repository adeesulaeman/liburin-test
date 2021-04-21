using MediatR;
using Service.Base;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Identity.Contracts;
using Service.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Queries.Roles
{
    public class GetRoleQuery:IRequest<PagedResultVM<RoleResponseVM>>
    {
        public PagedQueryVM PageQuery { get; set; }
        public SortableVM SortAble { get; set; }
    }

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, PagedResultVM<RoleResponseVM>>
    {
        private readonly IRoleRepository _roleRepository;
        public GetRoleQueryHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<PagedResultVM<RoleResponseVM>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Role, bool>> predicate = p => p.IsActive;

            if (!string.IsNullOrEmpty(request.PageQuery.Search))
                predicate = predicate.AndAlso(p => p.Name.ToLower().Contains(request.PageQuery.Search));

            var rawData = await _roleRepository.GetWithRelationsAsync(predicate);

            var totalRecord = rawData.Count();

            #region sort
            if (request.SortAble != null && request.SortAble.Field == "name")
            {
                if (request.SortAble.Order.Equals(-1))
                    rawData = rawData.OrderByDescending(x => x.Name);
                else
                    rawData = rawData.OrderBy(x => x.Name);
            }
            else
            {
                rawData = rawData.OrderByDescending(x => x.CreatedDate);
            }
            #endregion

            var resData = rawData
                .Skip((request.PageQuery.Page - 1) * request.PageQuery.ItemsPerPage)
                .Take(request.PageQuery.ItemsPerPage)
                .Select(x => new RoleResponseVM()
                {
                    Id = x.Id,
                    Name = x.Name,
                    GroupName = x.Group.Name
                });

            var result = new PagedResultVM<RoleResponseVM>()
            {
                CurrentPage = request.PageQuery.Page,
                ResultPerPage = request.PageQuery.ItemsPerPage,
                TotalRecords = totalRecord,
                Data = resData
            };

            return result;
        }
    }
}
