using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.DTOs.Subscription.Requests;
using SubscriptionPlatform.Application.Features.Subscriptions.Commands;
using SubscriptionPlatform.Application.Features.Subscriptions.Queries;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public SubscriptionController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles ="Admin,Customer")]
        public async Task<IActionResult> Create([FromBody] CreateSubscriptionRequest request)
        {
            var command = _mapper.Map<CreateSubscriptionCommand>(request);
            var subscriptionId = await _mediator.Send(command);

            return StatusCode(201, new { id = subscriptionId, message = "Abonelik başarıyla oluşturuldu." });  
        }

        [HttpPost("cancel/{id}")] //silmiyoruz pasif hale getiriyoruz
        [Authorize(Roles ="Admin,Customer")]
        public async Task<IActionResult> CancelSubscription(Guid id)
        {
            await _mediator.Send(new CancelSubscriptionCommand { SubscriptionId = id });
            return NoContent();
        }

        [HttpPost("renew/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProcessRenewal(Guid id)
        {
            await _mediator.Send(new ProcessRenewalCommand { SubscriptionId = id });
            return Ok(new { message = "Abonelik başarıyla yenilendi." });
        }

        [HttpGet("{id}")] //abonelik detayları
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetSubscriptionDetails(Guid id)
        {
            var result = await _mediator.Send(new GetSubscriptionDetailsQuery { SubscriptionId = id });
            return Ok(result);
        }

    }
}