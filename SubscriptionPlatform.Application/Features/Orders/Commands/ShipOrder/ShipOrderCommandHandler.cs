using MediatR;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.ShipOrder
{
    public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand, Unit>
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

        public async Task<Unit> Handle(ShipOrderCommand request, CancellationToken cancellationToken)
        {
            // 1. Siparişi ve Müşteriyi çekiyoruz
            var order = await _unitOfWork.Orders.GetOrderWithCustomerAsync(request.OrderId);

            if (order == null)
                throw new NotFoundException(nameof(Order), request.OrderId);

            if (order.Customer == null)
                throw new ApplicationException("Siparişe ait müşteri bilgisi bulunamadı.");
            var targetEmail = order.Customer.Email;

            var trackingNumber = await _fulfillmentService.CreateShipmentAsync(order);

            order.TrackingNumber = trackingNumber;
            order.Status = OrderStatus.Shipped;
            order.ShippingStatus = ShippingStatus.InTransit;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();

            if (!string.IsNullOrEmpty(targetEmail))
            {
                try
                {
                    await _emailService.SendEmailAsync(
                        targetEmail,
                        "Siparişiniz Yola Çıktı",
                        $"Merhaba, siparişiniz {trackingNumber} takip numarası ile kargoya verilmiştir.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Email gönderilemedi fakat sipariş süreci tamamlandı: {ex.Message}");
                }
            }

            return Unit.Value;
        }
    }
}