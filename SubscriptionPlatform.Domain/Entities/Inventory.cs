using SubscriptionPlatform.Domain.Common;

namespace SubscriptionPlatform.Domain.Entities
{
    public class Inventory : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int StockQuantity { get; set; } //stok miktarı
        public int LowStockThreshold { get; set; } //düşük stok
        public bool IsLowStockAlertSent { get; set; } = false; // bildirim kontrol
    }
}