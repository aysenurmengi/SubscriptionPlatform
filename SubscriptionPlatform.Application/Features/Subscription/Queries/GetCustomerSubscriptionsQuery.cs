using MediatR;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Queries
{
    public class GetCustomerSubscriptionsQuery : IRequest<IReadOnlyList<SubscriptionDetailDto>>
    {
        public Guid CustomerId { get; set; }
    }
}