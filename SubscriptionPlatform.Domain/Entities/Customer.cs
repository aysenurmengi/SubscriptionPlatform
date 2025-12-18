using SubscriptionPlatform.Domain.Common;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public string Email { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Preference> Preferences { get; set; } = new List<Preference>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}