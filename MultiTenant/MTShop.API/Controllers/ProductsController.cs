using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MTShop.Application.Commands.Products;
using MTShop.Application.Queries.Products;

namespace MTShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _mediator.Send(new ProductQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(await _mediator.Send(new ProductQueryById(id)));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] ProductCreatedCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
