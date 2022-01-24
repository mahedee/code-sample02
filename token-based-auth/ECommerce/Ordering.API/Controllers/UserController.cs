using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands.User.Create;
using Ordering.Application.Commands.User.Delete;
using Ordering.Application.DTOs;
using Ordering.Application.Queries.User;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> CreateUser(UserCreateCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(List<UserResponseDTO>))]
        public async Task<IActionResult> GetAllUserAsync()
        {
            return Ok(await _mediator.Send(new GetUserQuery()));
        }

        [HttpDelete("{userId}")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _mediator.Send(new DeleteUserCommand() { Id = userId});
            return Ok(result);
        }
    }
}
