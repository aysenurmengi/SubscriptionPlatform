using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands.CheckStock
{
    public class CheckLowStockCommandHandler : IRequestHandler<CheckLowStockCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CheckLowStockCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CheckLowStockCommand request, CancellationToken cancellationToken)
        {
            var lowStockItems = await _unitOfWork.Inventories.GetLowStockProductsAsync(0); 

            foreach (var item in lowStockItems)
            {
                var productName = item.Product?.Name ?? "Bilinmeyen Ürün";

                Console.WriteLine($"[KRİTİK STOK] Ürün ID: {productName} - Kalan Stok: {item.StockQuantity}");

                item.IsLowStockAlertSent = true;
                await _unitOfWork.Inventories.UpdateAsync(item);
                
            }

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
    
}