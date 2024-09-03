using Microsoft.AspNetCore.Mvc;
using webapisolution.Models;
using webapisolution.Services;

namespace webapisolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // Validate user credentials (replace with your actual validation logic)
            if (IsValidUser(model.Username, model.Password))
            {
                // Generate token
                var token = _tokenService.GenerateToken(model.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }

        private bool IsValidUser(string username, string password)
        {
            // Replace with your actual user validation logic (e.g., check against a database)
            return username == "testuser" && password == "testpassword";
        }
    }
}
