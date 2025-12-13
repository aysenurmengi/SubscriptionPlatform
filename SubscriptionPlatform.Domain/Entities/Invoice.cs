using SubscriptionPlatform.Domain.Common;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Invoice : BaseEntity
    { 
        public Guid SubscriptionId {get; set;}
        public decimal Amount {get; set;}
        public PaymentStatus PaymentStatus {get; set;}
        public DateTime IssueDate {get; set;}
        public DateTime DueDate {get; set;}

    }
}