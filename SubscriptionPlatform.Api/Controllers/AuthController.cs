using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.DTOs.Auth;
using SubscriptionPlatform.Application.Interfaces;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterUserAsync(
                request.Email, 
                request.Password, 
                request.FirstName, 
                request.LastName,
                request.Role);

            if (!result.IsSuccess)
            {
                return BadRequest(new { Errors = result.Errors });
            }

            return Ok(new { Message = "Kullanıcı başarıyla oluşturuldu." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _identityService.LoginAsync(request.Email, request.Password);

            if (token == null)
            {
                return Unauthorized(new { Message = "Email veya şifre hatalı." });
            }

            return Ok(new AuthResponse 
            { 
                Token = token, 
                Email = request.Email 
            });
        }

    }
}