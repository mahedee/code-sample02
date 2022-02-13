using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpGet("GetAll")]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "Soap", "Light", "Powder" };
        }
    }
}
