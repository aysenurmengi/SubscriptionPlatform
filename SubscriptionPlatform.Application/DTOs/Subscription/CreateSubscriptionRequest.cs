using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.DTOs.Subscription.Requests
{
    public class CreateSubscriptionRequest
    {
        public Guid CustomerId { get; set; }
        public Guid PlanId { get; set; }
        public int Quantity { get; set; }
        public string CardToken { get; set; } 
        public BillingCycle Cycle { get; set; }
        public string ShippingAddress { get; set; }
    }
}