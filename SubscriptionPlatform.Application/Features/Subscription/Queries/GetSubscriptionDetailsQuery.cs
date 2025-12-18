using MediatR;
using SubscriptionPlatform.Domain.Enums;
namespace SubscriptionPlatform.Application.Features.Subscriptions.Queries
{
    public class GetSubscriptionDetailsQuery : IRequest<SubscriptionDetailDto>
    {
        public Guid SubscriptionId { get; set; }
    }

    public class SubscriptionDetailDto
    {
        public Guid Id { get; set; }
        public string PlanName { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Quantity { get; set; }
        public SubscriptionStatus Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextRenewalDate { get; set; }
        public string ShippingAddress { get; set; }
        public BillingCycle Cycle { get; set; }
    }
}