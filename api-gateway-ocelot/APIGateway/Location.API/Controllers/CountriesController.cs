using Microsoft.AspNetCore.Mvc;

namespace Location.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
      [HttpGet("GetAll")]
      public IEnumerable<string> Get()
        {
            return new string[] {"America","Bangladesh", "Canada" };
        }
    }
}
