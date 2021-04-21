using MediatR;
using Service.Base.ViewModels.Common;
using Service.Identity.Contracts;
using Service.Identity.Models;
using Service.Identity.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Identity.CQRS.Queries.Users
{
    public class GetUserListQuery : IRequest<PagedResultVM<UsersResponseVM>>
    {
        public PagedQueryVM PageQuery { get; set; }
        public SortableVM SortAble { get; set; }
    }

    public class GetRoleQueryHandler : IRequestHandler<GetUserListQuery, PagedResultVM<UsersResponseVM>>
    {
        private readonly IUserRepository _userRepository;
        public GetRoleQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<PagedResultVM<UsersResponseVM>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<User, bool>> predicate = p => p.IsActive;

            // if (!string.IsNullOrEmpty(request.PageQuery.Search))
            //    predicate = predicate.AndAlso(p => p.Name.ToLower().Contains(request.PageQuery.Search));

            var rawData = await _userRepository.GetWithRelationsAsync(predicate);

            var totalRecord = rawData.Count();

            #region sort
            if (request.SortAble != null && request.SortAble.Field == "first_name")
            {
                if (request.SortAble.Order.Equals(-1))
                    rawData = rawData.OrderByDescending(x => x.Id);
                else
                    rawData = rawData.OrderBy(x => x.Id);
            }
            else
            {
                rawData = rawData.OrderByDescending(x => x.CreatedDate);
            }
            #endregion

            var resData = rawData
                .Skip((request.PageQuery.Page - 1) * request.PageQuery.ItemsPerPage)
                .Take(request.PageQuery.ItemsPerPage)
                .Select(x => new UsersResponseVM()
                {
                    Id = x.Id,
                    Firstname = x.FirstName,
                    Lastname = x.LastName,
                    Email = x.Email,
                    Phone = x.Phone,
                    GroupId = x.GroupId
                });

            var result = new PagedResultVM<UsersResponseVM>()
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
