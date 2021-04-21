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
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Identity.CQRS.Commands.Roles;
using Service.Identity.CQRS.Queries.Roles;

namespace Service.Identity.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserAuthProfile _user;
        public RoleController(IMediator mediator, IHttpContextAccessor accessor, IJwtHandler jwtHandler)
        {
            _mediator = mediator;

            if (accessor.HttpContext.User != null)
            {
                _user = jwtHandler.GetUserProfile(accessor.HttpContext);
            }

        }

        [HttpGet]
        public async Task<ActionResult> GetRoles([FromQuery]PagedQueryVM query, [FromQuery]SortableVM sortable)
        {
            try
            {
                var result = await _mediator.Send(new GetRoleQuery
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

        [HttpGet("{id:long}")]
        public async Task<ActionResult> GetRoleById(long id)
        {
            try
            {
                var result = await _mediator.Send(new GetRoleByIdQuery
                {
                    Id = id
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SuccessResponseVM>> CreateRole([FromBody] RoleVM role)
        {
            try
            {
                var result = await _mediator.Send(new CreateRoleCommand
                {
                    Payload = new CreateRoleRequestVM
                    {
                        Actor = new ProfileVM { Name= _user.Name },
                        Role = role
                    }
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<SuccessResponseVM>> UpdateRole([FromBody] RoleVM role, long id)
        {
            try
            {
                var result = await _mediator.Send(new UpdateRoleCommand
                {
                    Payload = role,
                    Actor = new ProfileVM { Name = _user.Name},
                    Id = id
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }


        [HttpDelete("{id:long}")]
        public async Task<ActionResult<SuccessResponseVM>> DeleteRole(long id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteRoleCommand
                {
                    Actor = new ProfileVM { Name = _user.Name },
                    Id = id
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
