using MediatR;

namespace SubscriptionPlatform.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerProfileCommand : IRequest<Unit> // geriye dönüş yok
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}