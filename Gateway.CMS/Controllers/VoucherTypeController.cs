using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Gateway.CMS.Contracts.RestEase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestEase;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Data;

namespace Gateway.CMS.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class VoucherTypeController : Controller
    {
        private readonly IVoucherTypeService _promoService;
        public VoucherTypeController(IVoucherTypeService promoService, IHttpContextAccessor accessor)
        {
            _promoService = promoService;
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponseVM>> CreateVocTypeAsync([FromBody] VoucherTypeVM requestVM)
        {
            var result = await _promoService.CreateVocType(requestVM);
            return ProcessResponse(result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultVM<VoucherTypeResponseVM>>> GetVocTypeListAsync([FromQuery] PagedQueryVM query, [FromQuery] SortableVM sortable)
        {
            var result = await _promoService.GetVocTypeList(query, sortable);
            return ProcessResponse(result);
        }
    }
}
