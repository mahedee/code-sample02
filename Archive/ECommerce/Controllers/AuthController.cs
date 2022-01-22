using ECommerce.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;

        public AuthController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginVM loginVM)
        {
            string token = string.Empty;

            token = jwtAuth.Authentication(loginVM.UserName, loginVM.Password);

            if(String.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
