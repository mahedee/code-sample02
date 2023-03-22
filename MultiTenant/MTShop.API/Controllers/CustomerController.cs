using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTShop.Application.Commands.Customer;
using MTShop.Application.DTOs;
using MTShop.Application.Queries.Customer;

namespace MTShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        [ProducesDefaultResponseType(typeof(IEnumerable<CustomerDTO>))]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new CustomerQuery()));
        }

        [HttpGet("GetById")]
        [ProducesDefaultResponseType(typeof(CustomerDTO))]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(await _mediator.Send(new CustomerQueryById(id)));
        }

        [HttpPost("Create")]
        [ProducesDefaultResponseType(typeof(CustomerDTO))]
        public async Task<IActionResult> CreateAsync([FromBody] CustomerCreatedCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}
