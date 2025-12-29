using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;
using SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder;
using SubscriptionPlatform.Application.Features.Invoices.Commands;
using SubscriptionPlatform.Application.Common.Exceptions;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public CreateSubscriptionCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, IMediator mediator, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<Guid> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.SubscriptionPlans.GetByIdWithItemsAsync(request.PlanId);

            if (plan == null)
                throw new NotFoundException(nameof(SubscriptionPlan), request.PlanId);

            foreach (var item in plan.PlanItems)
            {
                var inventory = await _unitOfWork.Inventories.GetByProductIdAsync(item.ProductId);

                if (inventory == null)
                    throw new NotFoundException("Inventory", item.ProductId);

                var requiredStock = item.Quantity * request.Quantity;

                if (inventory.StockQuantity < requiredStock)
                    throw new ApplicationException("Yetersiz stok");
            }

            var totalAmount = plan.Price * request.Quantity;

            var permanentToken = await _paymentService.StoreCreditCardAsync(request.CustomerId.ToString(), request.CardToken);

            if (string.IsNullOrEmpty(permanentToken))
                throw new ApplicationException("Kart saklama işlemi başarısız");

            var (isSuccess, transactionId, errorMessage) =
                await _paymentService.ProcessPaymentAsync(totalAmount, "EUR", permanentToken);

            if (!isSuccess)
                throw new ApplicationException($"Ödeme başarısız: {errorMessage}");

            var now = DateTime.UtcNow;

            var subscription = new Subscription
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                PlanId = plan.Id,
                PlanPrice = plan.Price,
                PaymentToken = permanentToken,
                Cycle = request.Cycle,
                StartDate = now,
                NextRenewalDate = now.AddMonths(1),
                Status = SubscriptionStatus.Active
            };

            await _unitOfWork.Subscriptions.AddAsync(subscription);

            foreach (var item in plan.PlanItems)
            {
                var inventory = await _unitOfWork.Inventories
                    .GetByProductIdAsync(item.ProductId);

                inventory.StockQuantity -= item.Quantity * request.Quantity;
            }

            await _mediator.Send(new GenerateInvoiceCommand
            {
                SubscriptionId = subscription.Id,
                Amount = totalAmount,
                PaymentTransactionId = transactionId
            }, cancellationToken);

            await _mediator.Send(new CreateOrderCommand
            {
                SubscriptionId = subscription.Id,
                CustomerId = request.CustomerId,
                IsSubscriptionRenewal = false,
                ShippingAddress = request.ShippingAddress
            }, cancellationToken);

            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);

            if (customer != null)
            {
                string subject = "Aboneliğiniz Başlatıldı 🎉";

                string body = $@"
                    <div style='font-family: Arial, sans-serif; line-height: 1.6'>
                        <h2>Aboneliğiniz başarıyla başlatıldı 🚀</h2>
                        <p>Merhaba <b>{customer.FirstName}</b>,</p>

                        <p>
                            Aboneliğiniz başarıyla oluşturulmuştur.
                            İlk siparişiniz hazırlanmak üzere sisteme alınmıştır.
                        </p>

                        <p>
                            <b>Abonelik No:</b> {subscription.Id}<br/>
                            <b>Paket:</b> {plan.Name}<br/>
                            <b>Toplam Tutar:</b> {totalAmount} ₺
                        </p>

                        <p>
                            Bundan sonraki dönemlerde aboneliğiniz otomatik olarak yenilenecek
                            ve her yenilemede yeni bir sipariş oluşturulacaktır.
                        </p>

                        <hr />
                        <small>Bu e-posta bilgilendirme amaçlıdır.</small>
                </div>";

                await _emailService.SendEmailAsync(
                    customer.Email,
                    subject,
                    body
                );
            }


            await _unitOfWork.CompleteAsync();

            return subscription.Id;
        }
    }
}