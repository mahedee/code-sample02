using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTShop.Application.Commands.User;
using MTShop.Application.DTOs;
using MTShop.Application.Queries.User;

namespace MTShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(List<UserResponseDTO>))]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new UsersQuery()));
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(UserDTO))]
        public async Task<IActionResult> CreateAsync([FromBody] UserCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{userId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteAsync(string userId)
        {
            return Ok(await _mediator.Send(new UserDeleteCommand(userId)));
        }
    }
}
