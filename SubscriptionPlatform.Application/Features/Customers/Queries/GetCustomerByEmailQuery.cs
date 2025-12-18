using MediatR;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public record GetCustomerByEmailQuery(string Email) : IRequest<CustomerDto>
    {
    }
}