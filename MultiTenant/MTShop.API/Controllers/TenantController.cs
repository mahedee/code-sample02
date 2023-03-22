using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTShop.Application.Commands.Tenant;
using MTShop.Application.Common.Constants;
using MTShop.Application.DTOs;
using MTShop.Application.Queries.Tenant;

namespace MTShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    ///Super Admin users are allow to CRUD Tenant Controller, Role name will read from token
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRolesConstants.SuperAdmin)]
    public class TenantController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TenantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new TenantQuery()));
        }

        [HttpPost("CreateTenant")]
        [ProducesDefaultResponseType(typeof(TenantDTO))]
        public async Task<IActionResult> CreateAsync([FromBody] TenantCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(Guid tenantId)
        {
            return Ok(await _mediator.Send(new TenantByIdQuery(tenantId)));
        }

        [HttpPut("EditTenant")]
        [ProducesDefaultResponseType(typeof(TenantDTO))]
        public async Task<IActionResult> UpdateAsync([FromBody] TenantUpdateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("Delete/{tenantId}")]
        public async Task<IActionResult> DeleteAsync(Guid tenantId)
        {
            return Ok(await _mediator.Send(new TenantDeleteCommand(tenantId)));
        }

    }
}
