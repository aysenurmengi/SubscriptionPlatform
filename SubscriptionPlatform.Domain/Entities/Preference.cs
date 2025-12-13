using SubscriptionPlatform.Domain.Common;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Preference : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        
        public Customer Customer { get; set; }
    }
}