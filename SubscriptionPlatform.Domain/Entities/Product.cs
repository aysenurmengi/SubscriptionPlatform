using SubscriptionPlatform.Domain.Common;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set;}
        public string ImageUrl { get; set; } = string.Empty;

        public Inventory Inventory { get; set; }
        public ICollection<SubscriptionPlanItem> PlanItems { get; set; } = new List<SubscriptionPlanItem>();

    }
}
