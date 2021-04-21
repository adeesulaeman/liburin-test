using MediatR;
using Service.Base;
using Service.Base.ViewModels.Common;
using Service.Data.Contracts;
using Service.Data.Models;
using Service.Data.ViewModels.VoucherType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Data.CQRS.Queries
{
    public class GetVoucherType : IRequest<PagedResultVM<VoucherTypeResponseVM>>
    {
        public PagedQueryVM PageQuery { get; set; }
        public SortableVM SortAble { get; set; }
    }

    public class GetVoucherTypeHandler : IRequestHandler<GetVoucherType, PagedResultVM<VoucherTypeResponseVM>>
    {
        private readonly IVoucherTypeRepository _vocTypeRepository;
        public GetVoucherTypeHandler(IVoucherTypeRepository vocTypeRepository)
        {
            _vocTypeRepository = vocTypeRepository;
        }
        public async Task<PagedResultVM<VoucherTypeResponseVM>> Handle(GetVoucherType request, CancellationToken cancellationToken)
        {
            Expression<Func<VoucherType, bool>> predicate = p => p.IsActive;

            if (!string.IsNullOrEmpty(request.PageQuery.Search))
                predicate = predicate.AndAlso(p => p.Name.ToLower().Contains(request.PageQuery.Search));

            var rawData = await _vocTypeRepository.GetWithRelationsAsync(predicate);

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
                .Select(x => new VoucherTypeResponseVM()
                {
                    Id = x.Id,
                    Name = x.Name
                });

            var result = new PagedResultVM<VoucherTypeResponseVM>()
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
