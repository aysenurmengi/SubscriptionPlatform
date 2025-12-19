using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.Subscriptions.Commands;
using SubscriptionPlatform.Application.Features.Subscriptions.Queries;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles ="Admin,Customer")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionCommand command)
        {
            try
            {
                var subscriptionId = await _mediator.Send(command);
                return Ok(new { 
                    _subscriptionId = subscriptionId, 
                    message = "Abonelik başarıyla başlatıldı. Hoş geldiniz!" 
                });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch(Exception ex)
            {//beklenmedik hatalar
                return StatusCode(500, new { message = "İşlem sırasında bir hata oluştu.", error = ex.Message });
            }
            
        }

        [HttpPost("cancel")] //silmiyoruz pasif hale getiriyoruz
        [Authorize(Roles ="Admin,Customer")]
        public async Task<IActionResult> CancelSubscription([FromBody] CancelSubscriptionCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok(new { message = "Abonelik başarıyla iptal edildi." });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = "İptal işlemi sırasında hata oluştu.", details = ex.Message });
            }
        }

        [HttpPost("renew/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessRenewal(Guid id)
        {
            var command = new ProcessRenewalCommand { SubscriptionId = id };
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { message = "Abonelik başarıyla yenilendi." });
            }
            else
            {
                return BadRequest(new { message = "Yenileme başarısız oldu. (Vakti gelmemiş olabilir veya ödeme alınamadı)" });
            }
        }

        [HttpGet("{id}")] //abonelik detayları
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetSubscriptionDetails(Guid id)
        {
            try
            {
                var query = new GetSubscriptionDetailsQuery { SubscriptionId = id };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Bir hata oluştu.", error = ex.Message });
            }
        }

    }
}