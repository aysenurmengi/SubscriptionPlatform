namespace SubscriptionPlatform.Domain.Enums
{
    public enum ShippingStatus
    {
        // sipariş alındı
        AwaitingFulfillment, 
        
        // hazırlanıyor
        InPreparation, 
        
        // kargo alındı
        Shipped, 
        
        // kargo süreci
        InTransit, 
        
        // teslim edildi
        Delivered, 
        
        // teslim edilemedi
        DeliveryFailed
    }
}