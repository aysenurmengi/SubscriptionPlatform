using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.DTOs.Auth;
using SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer;
using SubscriptionPlatform.Application.Interfaces;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public AuthController(IIdentityService identityService, IMediator mediator)
        {
            _identityService = identityService;
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerCommand command)
        {
            var resultId = await _mediator.Send(command);

            return Ok(new 
            { 
                Message = "Kullanıcı kaydı başarıyla tamamlandı.",
                Id = resultId
            });
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