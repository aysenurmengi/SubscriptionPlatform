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
            var invoiceId = await _mediator.Send(command);

            return StatusCode(201, new { invoiceId, message = "Fatura başarıyla oluşturuldu." });

        }

        [HttpGet("subscription/{subscriptionId}")]
        [Authorize(Roles = "Admin,Customer")] //müşteri de kendi faturasını görsün
        public async Task<IActionResult> GetInvoicesBySubscription(Guid subscriptionId)
        {
            var result = await _mediator.Send(new GetSubscriptionInvoicesQuery { SubscriptionId = subscriptionId });
            return Ok(result);
        }
    }
}