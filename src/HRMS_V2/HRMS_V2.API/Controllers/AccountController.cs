using HRMS_V2.API.Requests;
using HRMS_V2.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HRMS_V2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserModel>> AddUserAsync(AddUserRequest request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(RoleModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RoleModel>> AddRoleAsync(AddRoleRequest request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> AddToRoleAsync(AddUserToRoleRequest request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> AddRoleClaimsAsyc(AddRoleToClaimRequest request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}
