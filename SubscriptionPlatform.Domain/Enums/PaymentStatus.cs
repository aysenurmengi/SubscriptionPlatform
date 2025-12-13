namespace SubscriptionPlatform.Domain.Enums
{
    public enum PaymentStatus
    {
        // işlem henüz başlamadı
        Pending, 
        
        // başarılı
        Paid, 
        
        // başarısız
        Failed
    }
}