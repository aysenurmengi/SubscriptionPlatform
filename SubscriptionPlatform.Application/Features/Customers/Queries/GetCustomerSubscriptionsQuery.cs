using MediatR;
using SubscriptionPlatform.Domain.Enums;
using static SubscriptionPlatform.Application.Features.Customers.Queries.GetCustomerSubscriptionsQuery;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public class GetCustomerSubscriptionsQuery : IRequest<IReadOnlyList<SubscriptionDto>>
    {
        public Guid CustomerId { get; set; }

        public class SubscriptionDto
        {
            public Guid SubscriptionId { get; set; }
            public string PlanName { get; set; }
            public decimal Price { get; set; }
            public BillingCycle BillingCycle { get; set; }
            public SubscriptionStatus Status { get; set; }
            public DateTime NextRenewalDate { get; set; }
        }
    }
}