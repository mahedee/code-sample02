using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "Order 01", "Order 02", "Order 03" };
        }
    }
}
