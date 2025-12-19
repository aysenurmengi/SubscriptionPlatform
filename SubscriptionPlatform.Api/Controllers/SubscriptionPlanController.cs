using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.SubscriptionPlan.Commands;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionPlanController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionPlanCommand command)
        {
            var planId = await _mediator.Send(command);
            return Ok(new { planId, message = "Plan ve içeriği oluşturuldu." });
        }
    }
}
