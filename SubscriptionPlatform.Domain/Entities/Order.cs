using SubscriptionPlatform.Domain.Common;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Guid CustomerId {get; set;}
        public Customer Customer { get; set; }
        public Guid? SubscriptionId {get; set;}
        public virtual Subscription? Subscription { get; set; }
        public OrderStatus Status {get; set;}
        public string ShippingAddress {get; set;}
        public ShippingStatus ShippingStatus {get; set;}
        public string TrackingNumber {get; set;} = string.Empty;
        public bool IsSubscriptionRenewal {get; set;} = false; //tek seferlik mi? yenilenecek mi?
    }
}