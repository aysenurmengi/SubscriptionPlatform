using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using AutoMapper;

namespace SubscriptionPlatform.Application.Features.Inventory.Queries
{
    public class GetLowStockAlertsQueryHandler : IRequestHandler<GetLowStockAlertsQuery, IReadOnlyList<InventoryStatusDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetLowStockAlertsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<InventoryStatusDto>> Handle(GetLowStockAlertsQuery request, CancellationToken cancellationToken)
        {
            var allInventory = await _unitOfWork.Inventories.GetAllAsync();

            var lowStockItems = allInventory
                .Where(i => i.StockQuantity <= i.LowStockThreshold && !i.IsLowStockAlertSent)
                .ToList();

            return _mapper.Map<IReadOnlyList<InventoryStatusDto>>(lowStockItems);
        }
    }
}