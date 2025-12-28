using MediatR;

namespace SubscriptionPlatform.Application.Features.Customers.Queries.GetCustomers
{
    public record GetCustomersQuery : IRequest<List<CustomerDto>>;
}