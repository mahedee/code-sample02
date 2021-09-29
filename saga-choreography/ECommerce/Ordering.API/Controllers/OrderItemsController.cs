using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Ordering.API.Db;
using Ordering.API.Models;
using Plain.RabbitMQ;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly OrderingContext _context;
        private readonly IPublisher _publisher;

        public OrderItemsController(OrderingContext context, IPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        //// PUT: api/OrderItems/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutOrderItem(int id, OrderItem orderItem)
        //{
        //    if (id != orderItem.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(orderItem).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!OrderItemExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        // POST api/<OrderController>
        //[HttpPost]
        //public async Task<ActionResult> Post([FromBody] Order order)
        //{
        //    await _publishEndpoint.Publish<Order>(order);
        //    return Ok();
        //}



        // POST: api/OrderItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task PostOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            // New inserted identity value
            int id = orderItem.Id;


            _publisher.Publish(JsonConvert.SerializeObject(new OrderRequest
            {
                OrderId = orderItem.OrderId,
                CatalogId = orderItem.ProductId,
                Units = orderItem.Units,
                Name = orderItem.ProductName
            }),
            "order_created_routingkey", // Routing key
            null);
        }


        //// DELETE: api/OrderItems/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrderItem(int id)
        //{
        //    var orderItem = await _context.OrderItems.FindAsync(id);
        //    if (orderItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.OrderItems.Remove(orderItem);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool OrderItemExists(int id)
        //{
        //    return _context.OrderItems.Any(e => e.Id == id);
        //}
    }
}
