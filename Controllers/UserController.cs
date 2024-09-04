using Microsoft.AspNetCore.Mvc;
using webapisolution.Models;
using webapisolution.Repositories;
using webapisolution.Services;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public UserController(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel loginModel)
    {
        if (loginModel == null || string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
        {
            return BadRequest("Invalid login attempt.");
        }

        var user = await Task.Run(() => _userRepository.ValidateUser(loginModel.Username, loginModel.Password));

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _tokenService.GenerateToken(user.Username);

        return Ok(new { Token = token, user=user.Username });
    }
}
