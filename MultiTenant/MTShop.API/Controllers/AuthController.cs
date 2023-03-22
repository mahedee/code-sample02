using MediatR;
using Microsoft.AspNetCore.Mvc;
using MTShop.Application.Commands.Auth;

namespace MTShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Login for Admin service
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost("LoginAsAdmin")]
        public async Task<IActionResult> SignInAdmin([FromBody] AdminAuthCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Login for Tenant service, please provide tenant key at header
        /// </summary>
        /// <param name="command"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        
        [HttpPost("LoginAsTenant")]
        public async Task<IActionResult> SignIn([FromBody] AuthCommand command, [FromHeader] string tenant )
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
