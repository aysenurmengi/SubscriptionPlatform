using SubscriptionPlatform.Domain.Common;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime NextRenewalDate { get; set; }
        public BillingCycle Cycle { get; set; }
        public SubscriptionStatus Status { get; set; }

        public Customer Customer { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
} 