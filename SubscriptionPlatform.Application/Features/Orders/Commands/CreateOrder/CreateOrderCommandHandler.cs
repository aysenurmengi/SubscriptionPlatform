using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                SubscriptionId = request.SubscriptionId,
                ShippingAddress = request.ShippingAddress,
                IsSubscriptionRenewal = request.IsSubscriptionRenewal,
                
                Status = OrderStatus.Created,
                ShippingStatus = ShippingStatus.AwaitingFulfillment,
                TrackingNumber = string.Empty // Takip numarası daha sonra atanacak
            };
            
            await _unitOfWork.Orders.AddAsync(newOrder);
            await _unitOfWork.CompleteAsync();

            return newOrder.Id;
        }
    }
}