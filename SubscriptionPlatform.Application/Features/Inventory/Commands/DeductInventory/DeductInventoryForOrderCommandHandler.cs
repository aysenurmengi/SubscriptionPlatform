using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands
{
    public class DeductInventoryForOrderCommandHandler : IRequestHandler<DeductInventoryForOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeductInventoryForOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeductInventoryForOrderCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.ItemsToDeduct)
            {
                var productId = item.Key;
                var quantityToDeduct = item.Value;

                var inventory = await _unitOfWork.Inventories.GetByProductIdAsync(productId);

                if (inventory == null)
                {
                    throw new ApplicationException($"Envanter kaydı bulunamadı: Product ID {productId}");
                }

                if (inventory.StockQuantity < quantityToDeduct)
                {
                    throw new ApplicationException($"Stok yetersiz: {inventory.StockQuantity} mevcut, {quantityToDeduct} gerekli (Product ID: {productId}).");
                }

                inventory.StockQuantity -= quantityToDeduct;
                await _unitOfWork.Inventories.UpdateAsync(inventory);
            }
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}