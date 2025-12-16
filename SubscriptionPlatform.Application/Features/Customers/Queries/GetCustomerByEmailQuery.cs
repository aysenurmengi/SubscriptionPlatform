using MediatR;
using SubscriptionPlatform.Application.Features.Queries;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public class GetCustomerByEmailQuery(string Email) : IRequest<CustomerDto>
    {
        public string Email { get; set; }
    }
}