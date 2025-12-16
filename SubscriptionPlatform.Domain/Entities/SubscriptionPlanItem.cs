using SubscriptionPlatform.Domain.Common;
using System;

namespace SubscriptionPlatform.Domain.Entities
{
    // abonelik planındaki ürünler için ara tablo, ürün ve sayısı
    public class SubscriptionPlanItem : BaseEntity
    {
        public Guid SubscriptionPlanId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public SubscriptionPlan SubscriptionPlan { get; set; }
        public Product Product { get; set; } 
    }
}