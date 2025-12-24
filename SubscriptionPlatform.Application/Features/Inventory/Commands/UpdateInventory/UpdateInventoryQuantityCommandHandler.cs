using MediatR;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using InventoryEntity = SubscriptionPlatform.Domain.Entities.Inventory;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands
{
    public class UpdateInventoryQuantityCommandHandler : IRequestHandler<UpdateInventoryQuantityCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateInventoryQuantityCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateInventoryQuantityCommand request, CancellationToken cancellationToken)
        {
            var inventory = await _unitOfWork.Inventories.GetByProductIdAsync(request.ProductId);

            if (inventory == null)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId);
                if (product == null)
                    throw new NotFoundException(nameof(Product), request.ProductId);

                inventory = new InventoryEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    StockQuantity = request.NewStockQuantity,
                    LowStockThreshold = request.NewLowStockThreshold,
                };

                await _unitOfWork.Inventories.AddAsync(inventory);
            }else
            {
                inventory.StockQuantity = request.NewStockQuantity;
                inventory.LowStockThreshold = request.NewLowStockThreshold;

                if (inventory.StockQuantity > inventory.LowStockThreshold)
                    {
                        inventory.IsLowStockAlertSent = false;
                    }
        
                await _unitOfWork.Inventories.UpdateAsync(inventory);
            }

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}