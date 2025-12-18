using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.Invoices.Commands;
using SubscriptionPlatform.Application.Features.Invoices.Queries;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GenerateInvoiceCommand command)
        {
            try
            {
                var invoiceId = await _mediator.Send(command);
                return Ok(new { invoiceId = invoiceId, message = "Fatura başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Fatura oluşturulurken hata oluştu.", error = ex.Message });
            }
            
        }

        [HttpGet("subscription/{subscriptionId}")]
        [Authorize(Roles = "Admin,Customer")] //müşteri de kendi faturasını görsün
        public async Task<IActionResult> GetInvoicesBySubscription(Guid subscriptionId)
        {
            var query = new GetSubscriptionInvoicesQuery { SubscriptionId = subscriptionId };
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}