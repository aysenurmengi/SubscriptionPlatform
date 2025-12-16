using SubscriptionPlatform.Domain.Common;

namespace SubscriptionPlatform.Domain.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public ICollection<SubscriptionPlanItem> PlanItems { get; set; } = new List<SubscriptionPlanItem>();
    }
}