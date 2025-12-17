using MediatR;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.ShipOrder
{
    public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFulfillmentService _fulfillmentService;
        private readonly IEmailService _emailService;

        public ShipOrderCommandHandler(IUnitOfWork unitOfWork, IFulfillmentService fulfillmentService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _fulfillmentService = fulfillmentService;
            _emailService = emailService;
        }

        public async Task<bool> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId);
            
            if (order == null || order.Customer == null) return false;

            var trackingNumber = await _fulfillmentService.CreateShipmentAsync(order);

            order.TrackingNumber = trackingNumber;
            order.Status = OrderStatus.Shipped;
            order.ShippingStatus = ShippingStatus.InTransit;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();

            await _emailService.SendEmailAsync(
                order.Customer.Email,
                "Siparişiniz Yola Çıktı", 
                $"Merhaba, siparişiniz {trackingNumber} takip numarası ile kargoya verilmiştir.");

            return true;
        }
    }
}