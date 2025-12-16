using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Domain.Enums;
using SubscriptionPlatform.Application.Features.Invoices.Commands;
using SubscriptionPlatform.Application.Features.Orders.Commands; 
using System;
using System.Threading;
using System.Threading.Tasks;
using SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class ProcessRenewalCommandHandler : IRequestHandler<ProcessRenewalCommand, bool>
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

        public async Task<bool> Handle(ProcessRenewalCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.SubscriptionId);
            if (subscription == null || subscription.Status != SubscriptionStatus.Active)
            {
                return false; 
            }
            // Yeni EKLENTİ: En güncel gönderim adresini son siparişten çekme
            // Varsayım: Repository'de müşteriye ait son Order'ı çeken bir metot var.
            //var latestOrder = await _unitOfWork.Orders.GetLatestOrderByCustomerIdAsync(subscription.CustomerId);
            // Eğer hiç siparişi yoksa (ki abonelik varsa olmalı), Address'i boş bırakabiliriz (veya hata verebiliriz).
            //var currentShippingAddress = latestOrder?.ShippingAddress ?? "Adres Bilgisi Eksik";

            
            if (subscription.NextRenewalDate > DateTime.UtcNow)
            {
                return false; // yenileme zamanı gelmemiş
            }

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
                return true;
                
            }
            else
            {
                subscription.Status = SubscriptionStatus.PaymentFailed;
                await _unitOfWork.CompleteAsync();

                return false;
            }
        }
    }
}