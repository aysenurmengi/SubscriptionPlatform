using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.Preferences.Commands;
using SubscriptionPlatform.Application.Features.Preferences.Queries;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PreferenceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PreferenceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> UpdatePreferences([FromBody] UpdateCustomerPreferencesCommand command)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Customer" && userId != null)
                {
                    if (command.CustomerId.ToString() != userId)
                    {
                        return Unauthorized(new { message = "Sadece kendi tercihlerinizi güncelleyebilirsiniz." });
                    }
                }

                await _mediator.Send(command);

                return Ok(new { message = "Tercihler başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Tercihler güncellenirken hata oluştu.", error = ex.Message });
            }
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetCustomerPreferences(Guid customerId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Customer" && userId != null)
                {
                    if (customerId.ToString() != userId)
                    {
                        return Unauthorized(new { message = "Sadece kendi tercihlerinizi görüntüleyebilirsiniz." });
                    }
                }

                var query = new GetCustomerPreferencesQuery { CustomerId = customerId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Tercihler getirilirken bir hata oluştu.", error = ex.Message });
            }
        }
    }
}