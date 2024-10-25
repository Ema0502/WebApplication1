using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace WebApplication1.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly string secretKey;

        public AuthenticationController(IConfiguration config)
        {
            secretKey = config.GetSection("Settings").GetSection("SecretKey").ToString();
        }

        [HttpPost]
        [Route("validate")]
        public IActionResult validate([FromBody] User request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            //if (request.Email)
        }
    }
}
