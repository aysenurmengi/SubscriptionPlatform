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
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sunucu hatası oluştu.", details = ex.Message });
            }
        }

        [HttpPut("update-quantity")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStock([FromBody] UpdateInventoryQuantityCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Stok güncellenirken hata oluştu.", details = ex.Message });
            }
        }

        [HttpGet("low-stock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLowStockAlerts()
        {
            var query = new GetLowStockAlertsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }






    }
}