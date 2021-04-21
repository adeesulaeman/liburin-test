using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Base.Auth.Contracts;
using Service.Base.Auth.Models;
using Service.Identity.CQRS.Queries.Master;

namespace Service.Identity.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserAuthProfile _user;

        public MasterController(IMediator mediator, IHttpContextAccessor accessor, IJwtHandler jwtHandler)
        {
            _mediator = mediator;

            if (accessor.HttpContext.User != null)
            {
                _user = jwtHandler.GetUserProfile(accessor.HttpContext);
            }
        }

        [HttpGet("GetMasterUserType")]
        public async Task<ActionResult> GetMasterUserType()
        {
            try
            {
                var result = await _mediator.Send(new GetMasterUserTypeQuery{});

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet("GetMasterGroupUser")]
        public async Task<ActionResult> GetMasterGroupUser()
        {
            try
            {
                var result = await _mediator.Send(new GetMasterGroupUserQuery { });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet("GetMasterRole")]
        public async Task<ActionResult> GetMasterRole()
        {
            try
            {
                var result = await _mediator.Send(new GetMasterRoleQuery { });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet("GetMasterModule")]
        public async Task<ActionResult> GetMasterModule()
        {
            try
            {
                var result = await _mediator.Send(new GetMasterModulesQuery { });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet("GetMasterOperation")]
        public async Task<ActionResult> GetMasterOperation()
        {
            try
            {
                var result = await _mediator.Send(new GetMasterOperationsQuery { });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
