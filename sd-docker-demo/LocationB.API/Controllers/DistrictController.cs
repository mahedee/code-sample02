using Microsoft.AspNetCore.Mvc;

namespace LocationB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        // GET: api/<DistrictController>
        [HttpGet("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Serivce:LocationB.API->", "Kumilla", "Bogura", "Natore", "Kurigram", "Natore" };
        }

        // GET api/<DistrictController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<DistrictController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DistrictController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DistrictController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
