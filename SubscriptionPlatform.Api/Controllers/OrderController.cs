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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Customer" && userId != null && command.CustomerId.ToString() != userId)
            {
                return Unauthorized(new { message = "Sadece kendi hesabınız için sipariş oluşturabilirsiniz." });
            }

            var orderId = await _mediator.Send(command);

            return Ok(new 
            { 
                Id = orderId, 
                message = "Siparişiniz başarıyla alındı.",
                status = "Hazırlanıyor"
            });
        }

        [HttpPost("{id}/ship")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ShipOrder(Guid id)
        {
            await _mediator.Send(new ShipOrderCommand(id));
        
            return Ok(new { message = "Sipariş kargoya verildi, takip numarası oluşturuldu ve müşteriye e-posta atıldı." });
        }

        [HttpGet("customer/{customerId}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetCustomerOrders(Guid customerId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Customer" && userId != null && customerId.ToString() != userId)
            {
                return Unauthorized(new { message = "Sadece kendi sipariş geçmişinizi görüntüleyebilirsiniz." });
            }

            var result = await _mediator.Send(new GetCustomerOrdersQuery { CustomerId = customerId });
            return Ok(result);
        }
    }

    
}