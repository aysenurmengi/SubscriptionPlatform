using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubscriptionPlatform.Application.DTOs.Customers;
using SubscriptionPlatform.Application.Features.Customers.Queries;
using SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer;
using SubscriptionPlatform.Application.Features.Customers.Commands.UpdateCustomer;
using SubscriptionPlatform.Application.Features.Customers.Commands.DeleteCustomer;

namespace SubscriptionPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
        {
            var command = new RegisterCustomerCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password

            };

            try
            {
                var customerId = await _mediator.Send(command);
               
                return StatusCode(201, new { id = customerId });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _mediator.Send(query);

            if (customer == null)
            {
                return NotFound(new { message = "Aradığınız kullanıcı bulunamadı." });
            }

            return Ok(customer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            var command = new UpdateCustomerProfileCommand
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email
            };

            try
            {
                await _mediator.Send(command);
                
                return NoContent(); // 204
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteCustomerCommand {Id = id};
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{customerId}/subscriptions")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetSubscriptions(Guid customerId)
        {
            var query = new GetCustomerSubscriptionsQuery { CustomerId = customerId };
    
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("by-email")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmail([FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email adresi boş olamaz.");
            }

            var query = new GetCustomerByEmailQuery(email);
            var customer = await _mediator.Send(query);

            if (customer == null)
            {
                return NotFound("Bu email adresine sahip müşteri bulunamadı.");
            }

            return Ok(customer);
        }
    }
}