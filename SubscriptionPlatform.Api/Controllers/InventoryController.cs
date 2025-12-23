using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.Inventory.Commands;
using SubscriptionPlatform.Application.Features.Inventory.Queries;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("deduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deduct([FromBody] DeductInventoryForOrderCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPut("update-quantity")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateInventoryQuantityCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLowStockAlerts()
        {
            var result = await _mediator.Send(new GetLowStockAlertsQuery());
            return Ok(result);
        }

    }
}