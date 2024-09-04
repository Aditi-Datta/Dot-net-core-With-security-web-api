using Microsoft.AspNetCore.Mvc;
using webapisolution.Models;
using webapisolution.Services;
using webapisolution.Repositories;
using System.Threading.Tasks;

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
        public async Task<MessageStatus> Login([FromBody] LoginModel model)
        {
            var messegeStatus = new MessageStatus();

            try
            {
                if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                {
                    return new MessageStatus
                    {
                        Status = false,
                        Code = 400, // Bad Request
                        Message = "Username and password are required."
                    };
                }

                // Retrieve user info
                var user = _userRepository.ValidateUser(model.Username, model.Password);

                if (user != null)
                {
                    // Generate token
                    var token = _tokenService.GenerateToken(user.Username);

                    // Return success message
                    messegeStatus = new MessageStatus
                    {
                        Data = new
                        {
                            Token = token,
                            FullName = user.FullName,
                            IsActive = user.IsActive,
                            email=user.Email,
                        },
                        Status = true,
                        Code = 200, // OK
                        Message = "Successfully Logged In"
                    };
                }
                else
                {
                    // User not found or invalid credentials
                    messegeStatus = new MessageStatus
                    {
                        Status = false,
                        Code = 401, // Unauthorized
                        Message = "Invalid username or password."
                    };
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                // _logger.LogError(ex, "An error occurred during login.");

                messegeStatus = new MessageStatus
                {
                    Status = false,
                    Code = 500, // Internal Server Error
                    Message = "An error occurred during login."
                };
            }

            return messegeStatus;
        }
    }
}
