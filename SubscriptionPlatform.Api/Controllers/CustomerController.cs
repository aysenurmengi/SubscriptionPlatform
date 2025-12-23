using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.DTOs.Customers;
using SubscriptionPlatform.Application.Features.Customers.Queries;
using SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer;
using SubscriptionPlatform.Application.Features.Customers.Commands.UpdateCustomer;
using SubscriptionPlatform.Application.Features.Customers.Commands.DeleteCustomer;
using AutoMapper;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CustomerController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            var command = _mapper.Map<RegisterCustomerCommand>(request);
            var customerId = await _mediator.Send(command);
    
            return StatusCode(201, new { id = customerId });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery(id));

            return Ok(customer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            var command = _mapper.Map<UpdateCustomerProfileCommand>(request);
            command.Id = id;

            await _mediator.Send(command);
    
            return NoContent(); // 204
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteCustomerCommand { Id = id });
            return NoContent();
        }

        [HttpGet("{customerId}/subscriptions")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetSubscriptions(Guid customerId)
        {
            var result = await _mediator.Send(new GetCustomerSubscriptionsQuery { CustomerId = customerId });
            return Ok(result);
        }

        [HttpGet("by-email")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));
            return Ok(customer);
        }
    }
}