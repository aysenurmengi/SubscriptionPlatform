using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.Features.Personalization.Queries.GetPersonalizedProducts;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonalizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("recommendations/{customerId}")]
        public async Task<IActionResult> GetRecommendations(Guid customerId)
        {
            var result = await _mediator.Send(new GetPersonalizedProductsQuery(customerId));
            return Ok(result);
        }
    }
}