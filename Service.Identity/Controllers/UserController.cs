using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Base.Auth.Models;
using Service.Base.ViewModels.Common;
using Service.Base.ViewModels.Identity;
using Service.Identity.CQRS.Commands.Users;
using Service.Identity.CQRS.Queries.Users;
using Service.Identity.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Service.Identity.Controllers
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<SuccessResponseVM>> CreateUser([FromBody] UserRequestVM user)
        {
            try
            {
                var userActor = new ProfileVM()
                {
                    Name = "system"
                };
                var result = await _mediator.Send(new CreateUserCommand
                {
                    Payload = new CreateUserVM
                    {
                        Actor = userActor,
                        User = user
                    }
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetRoles([FromQuery] PagedQueryVM query, [FromQuery] SortableVM sortable)
        {
            try
            {
                var result = await _mediator.Send(new GetUserListQuery
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

        [HttpPost("Login")]
        public async Task<ActionResult<SuccessResponseVM>> CreateRoleAsync([FromBody] UserLoginVM userLogin)
        {
            try
            {
                var result = await _mediator.Send(new UserLoginCommand
                {
                    Payload = userLogin
                });

                if (result.IsSuccess)
                    return Ok(result);
                else
                    return BadRequest(result.Reason);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpPost("Token")]
        public async Task<ActionResult<JsonWebToken>> GetTokenAsync([FromBody] UserLoginVM userLogin)
        {
            try
            {
                var result = await _mediator.Send(new GetTokenQuery
                {
                    Payload = userLogin
                });

                return Ok(result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet("RefreshToken")]
        public async Task<ActionResult<JsonWebToken>> GetTokenAsync([FromQuery] string refreshToken)
        {
            try
            {
                var result = await _mediator.Send(new RefreshTokenQuery
                {
                    RefreshToken = refreshToken
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
