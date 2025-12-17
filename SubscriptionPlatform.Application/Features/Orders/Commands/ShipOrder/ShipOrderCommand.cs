using MediatR;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.ShipOrder
{
    public record ShipOrderCommand(Guid OrderId) : IRequest<bool>;
}