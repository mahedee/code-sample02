using Microsoft.AspNetCore.Mvc;

namespace Location.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
      [HttpGet]
      public IEnumerable<string> Get()
        {
            return new string[] {"America","Bangladesh", "Canada" };
        }
    }
}
