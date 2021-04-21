using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Base.Auth.Contracts;
using Service.Base.Auth.Models;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Data.Contracts;
using Service.Data.CQRS.Commands;
using Service.Data.CQRS.Queries;
using Service.Data.Models;
using Service.Data.ViewModels.VoucherType;

namespace Service.Data.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VoucherTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public VoucherTypeController(IMediator mediator, IHttpContextAccessor accessor)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetVoucherType([FromQuery] PagedQueryVM query, [FromQuery] SortableVM sortable)
        {
            try
            {
                var result = await _mediator.Send(new GetVoucherType
                {
                    PageQuery = query,
                    SortAble = sortable
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponseVM>> CreateVoucherType([FromBody] VoucherTypeVM vocType)
        {
            try
            {
                var result = await _mediator.Send(new CreateVoucherType
                {
                    Payload = new CreateVoucherTypeRequestVM
                    {
                        VoucherType = vocType
                    },

                    Actor = new ProfileVM { Name = "System" } 
                });;

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
