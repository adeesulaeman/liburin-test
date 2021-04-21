using MediatR;
using Service.Base;
using Service.Base.ViewModels.Common;
using Service.Data.Contracts;
using Service.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Data.CQRS.Queries
{
    public class GetPromoQuery : IRequest<PagedResultVM<Promo>>
    {
        public PagedQueryVM PageQuery { get; set; }
        public SortableVM SortAble { get; set; }
    }

    public class GetPromoQueryHandler : IRequestHandler<GetPromoQuery, PagedResultVM<Promo>>
    {
        private readonly IPromoRepository _promoRepository;
        public GetPromoQueryHandler(IPromoRepository promoRepository)
        {
            _promoRepository = promoRepository;
        }
        public async Task<PagedResultVM<Promo>> Handle(GetPromoQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Promo, bool>> predicate = p => p.IsActive;

            if (!string.IsNullOrEmpty(request.PageQuery.Search))
                predicate = predicate.AndAlso(p => p.Title.ToLower().Contains(request.PageQuery.Search));

            var rawData = await _promoRepository.GetWithRelationsAsync(predicate);

            var totalRecord = rawData.Count();

            #region sort
            if (request.SortAble != null && request.SortAble.Field == "title")
            {
                if (request.SortAble.Order.Equals(-1))
                    rawData = rawData.OrderByDescending(x => x.Title);
                else
                    rawData = rawData.OrderBy(x => x.Title);
            }
            else
            {
                rawData = rawData.OrderByDescending(x => x.CreatedDate);
            }
            #endregion

            var resData = rawData
                .Skip((request.PageQuery.Page - 1) * request.PageQuery.ItemsPerPage)
                .Take(request.PageQuery.ItemsPerPage)
                .ToList();

            var result = new PagedResultVM<Promo>()
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
