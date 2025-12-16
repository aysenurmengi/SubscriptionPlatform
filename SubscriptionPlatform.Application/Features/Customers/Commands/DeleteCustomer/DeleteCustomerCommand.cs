using MediatR;

namespace SubscriptionPlatform.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
