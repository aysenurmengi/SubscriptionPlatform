using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

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
                throw new ApplicationException($"Envanter kaydı bulunamadı: Product ID {request.ProductId}");
            }

            inventory.StockQuantity = request.NewStockQuantity;
            
            _unitOfWork.Inventories.UpdateAsync(inventory);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}