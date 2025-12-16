using MediatR;

namespace SubscriptionPlatform.Application.Features.Queries
{
    public class GetCustomerByIdQuery(Guid CustomerId) : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime MemberSince { get; set; } 
    }
}