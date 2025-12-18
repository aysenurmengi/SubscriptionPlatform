using MediatR;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public record GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDto>
    {}
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime MemberSince { get; set; } 
    }
}