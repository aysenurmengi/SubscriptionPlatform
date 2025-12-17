using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Infrastructure.Services
{
    public class FulfillmentService : IFulfillmentService
    {
        public async Task<string> CreateShipmentAsync(Order order)
        {
            await Task.Delay(1000); // fake bekleme süresi

            // takip numarası üretme örn:SP-2023-XXXX - fake
            string trackingNumber = $"SP-{DateTime.Now.Year}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            return trackingNumber;
        }

        public async Task<ShippingStatus> GetShippingStatusAsync(string trackingNumber)
        {
            await Task.Delay(500); // yine fake sorgulama süresi

            return ShippingStatus.InTransit; //rastgele 
        }
    }
}