using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands.Customers;
using Ordering.Application.DTOs;
using Ordering.Application.Queries;
using Ordering.Core.Entities;

namespace Ordering.API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin,Member")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<List<Customer>> Get()
        {
            return await _mediator.Send(new GetAllCustomerQuery());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Customer> Get(Int64 id)
        {
            return await _mediator.Send(new GetCustomerByIdQuery(id));
        }

        [HttpGet("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<Customer> GetByEmail(string email)
        {
            return await _mediator.Send(new GetCustomerByEmailQuery(email));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpPut("EditCustomer/{id}")]
        public async Task<ActionResult> EditCustomer(int id, [FromBody] EditCustomerCommand command)
        {
            try
            {
                if (command.Id == id)
                {
                    var result = await _mediator.Send(command);
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            try
            {
                string result = string.Empty;
                result = await _mediator.Send(new DeleteCustomerCommand(id));
                return Ok(result);
            }
            catch (Exception exp)
            {
                return BadRequest(exp.Message);
            }
        }

    }
}
