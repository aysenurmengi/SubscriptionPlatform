using MediatR;

namespace SubscriptionPlatform.Application.Features.Inventory.Queries
{
    public class GetLowStockAlertsQuery : IRequest<IReadOnlyList<InventoryStatusDto>>
    {}
    public class InventoryStatusDto
    {
        public Guid ProductId { get; set; }
        public int StockQuantity { get; set; }
        public int LowStockThreshold { get; set; }
        public bool IsLowStock => StockQuantity <= LowStockThreshold;
    }
}