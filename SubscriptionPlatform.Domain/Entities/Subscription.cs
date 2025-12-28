using System.ComponentModel.DataAnnotations.Schema;
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
        public Guid PlanId { get; set; } // hangi ürüne abone?
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public string PaymentToken { get; set; }  //tekrarlayan ödeme için token
        public decimal PlanPrice { get; set; } // abonelik başlangıç fiyatı

        public Customer Customer { get; set; }
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
} 