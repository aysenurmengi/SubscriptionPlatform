using MediatR;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands.CheckStock
{
    public record CheckLowStockCommand : IRequest<Unit>;
}