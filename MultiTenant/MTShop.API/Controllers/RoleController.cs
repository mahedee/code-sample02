using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTShop.Application.Commands.Role;
using MTShop.Application.DTOs;
using MTShop.Application.Queries.Role;

namespace MTShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(IEnumerable<RoleDTO>))]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new RolesQuery()));
        }

        [HttpGet("{roleId}")]
        [ProducesDefaultResponseType(typeof(RoleDTO))]
        public async Task<IActionResult> GetAsync(string roleId)
        {
            return Ok(await _mediator.Send(new RoleByIdQuery(roleId)));
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(RoleDTO))]
        public async Task<IActionResult> CreateAsync([FromBody] RoleCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut]
        [ProducesDefaultResponseType(typeof(RoleDTO))]
        public async Task<IActionResult> UpdateAsync([FromBody] RoleUpdateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{roleId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteAsync(string roleId)
        {
            return Ok(await _mediator.Send(new RoleDeleteCommand(roleId)));
        }
    }
}
