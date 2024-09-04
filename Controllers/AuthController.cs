using Microsoft.AspNetCore.Mvc;
using webapisolution.Models;
using webapisolution.Services;
using webapisolution.Repositories;

namespace webapisolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public AuthController(TokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = _userRepository.ValidateUser(model.Username, model.Password);
            if (user != null)
            {
                var token = _tokenService.GenerateToken(user.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid username or password.");
        }
    }
}
