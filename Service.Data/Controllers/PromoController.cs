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
using Service.Base.Contracts;
using Service.Base.ViewModels.Identity;
using Service.Data.CQRS.Commands;
using Service.Data.Models;

namespace Service.Data.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PromoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PromoController(IMediator mediator, IHttpContextAccessor accessor)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Promo>> CreatePromo([FromBody] Promo promo)
        {
            try
            {
                var result = await _mediator.Send(new CreatePromo
                {
                    Payload = promo,
                    Actor = new ProfileVM { Name = "System" }
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
