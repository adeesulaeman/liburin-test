using RestEase;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Gateway.CMS.Contracts.RestEase
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IVoucherTypeService
    {
        [AllowAnyStatusCode]
        [Post("v1/VoucherType")]
        Task<Response<SuccessResponseVM>> CreateVocType([Body] VoucherTypeVM role);

        [Get("v1/VoucherType")]
        Task<Response<PagedResultVM<VoucherTypeResponseVM>>> GetVocTypeList([Query] PagedQueryVM query, [Query] SortableVM sortable);
    }
}
