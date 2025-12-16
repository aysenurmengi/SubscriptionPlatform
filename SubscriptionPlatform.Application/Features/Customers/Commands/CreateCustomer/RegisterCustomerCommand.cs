using MediatR;

namespace SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer
{
    public class RegisterCustomerCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}