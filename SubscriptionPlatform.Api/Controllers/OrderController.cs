using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder;
using SubscriptionPlatform.Application.Features.Orders.Commands.ShipOrder;
using SubscriptionPlatform.Application.Features.Orders.Queries;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Customer" && userId != null)
                {
                    if (command.CustomerId.ToString() != userId)
                    {
                        return Unauthorized(new { message = "Sadece kendi hesabınız için sipariş oluşturabilirsiniz." });
                    }
                }

                var orderId = await _mediator.Send(command);

                return Ok(new 
                {    
                    orderId = orderId, 
                    message = "Siparişiniz başarıyla alındı.",
                    status = "Hazırlanıyor"
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Sipariş oluşturulurken beklenmedik bir hata oluştu.", details = ex.Message });
            }
        }

        [HttpPost("{id}/ship")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShipOrder(Guid id)
        {
            var command = new ShipOrderCommand(id);
            
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { message = "Sipariş kargoya verildi, takip numarası oluşturuldu ve müşteriye e-posta atıldı." });
            }
            else
            {
                return BadRequest(new { message = "İşlem başarısız. Sipariş bulunamadı veya müşteri bilgisi eksik." });
            }
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Customer" && userId != null)
            {
                if (customerId.ToString() != userId)
                {
                    return Unauthorized(new { message = "Sadece kendi sipariş geçmişinizi görüntüleyebilirsiniz." });
                }
            }

            var query = new GetCustomerOrdersQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }

    
}