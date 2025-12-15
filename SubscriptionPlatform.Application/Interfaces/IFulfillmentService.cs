using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Interfaces
{
    public interface IFulfillmentService
    {
        // order nesnesini al, firmaya ilet, takip numarası dön
        Task<string> CreateShipmentAsync(Order order);

        // takip numarasına göre gönderim durumunu
        Task<Domain.Enums.ShippingStatus> GetShippingStatusAsync(string trackingNumber);
    }
}