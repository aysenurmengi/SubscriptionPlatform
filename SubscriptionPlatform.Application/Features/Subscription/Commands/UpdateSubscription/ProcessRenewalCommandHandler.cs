using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Domain.Enums;
using SubscriptionPlatform.Application.Features.Invoices.Commands;
using SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class ProcessRenewalCommandHandler : IRequestHandler<ProcessRenewalCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator; 

        public ProcessRenewalCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProcessRenewalCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.SubscriptionId);

            if (subscription == null || subscription.Status != SubscriptionStatus.Active)
                throw new NotFoundException(nameof(Subscription), request.SubscriptionId);

            
            if (subscription.Status != SubscriptionStatus.Active)
                throw new ApplicationException("Sadece aktif abonelikler yenilenebilir.");

            if (subscription.NextRenewalDate > DateTime.UtcNow)
                throw new ApplicationException("Bu aboneliğin yenileme tarihi henüz gelmedi.");

            var totalAmount = subscription.PlanPrice;
            
            // tekrarlayan ödeme işlemi, token kullanarak
            var (isSuccess, transactionId, errorMessage) = 
                await _paymentService.ProcessPaymentAsync(totalAmount, "EUR", subscription.PaymentToken); 

            if (isSuccess)
            { //son siparişten shipping adres 
                var latestOrder = await _unitOfWork.Orders.GetLatestOrderByCustomerIdAsync(subscription.CustomerId);
                var currentShippingAddress = latestOrder?.ShippingAddress ?? "Adres Bilgisi Eksik";

                await _mediator.Send(new CreateOrderCommand 
                {
                    SubscriptionId = subscription.Id,
                    CustomerId = subscription.CustomerId,
                    IsSubscriptionRenewal = true, //yenileme old. belirtmek için
                    ShippingAddress = currentShippingAddress
                });

                await _mediator.Send(new GenerateInvoiceCommand 
                {
                    SubscriptionId = subscription.Id,
                    Amount = totalAmount,
                    PaymentTransactionId = transactionId
                });

                subscription.NextRenewalDate = subscription.NextRenewalDate.AddMonths(1); 
                subscription.Status = SubscriptionStatus.Active;

                await _unitOfWork.CompleteAsync();
                return Unit.Value;
                
            }
            else
            {
                subscription.Status = SubscriptionStatus.PaymentFailed;
                await _unitOfWork.CompleteAsync();

                throw new ApplicationException($"Yenileme ödemesi başarısız: {errorMessage}");
            }
        }
    }
}