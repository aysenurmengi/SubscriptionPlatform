using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;
using SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder;
using SubscriptionPlatform.Application.Features.Invoices.Commands;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class CreateSubscriptionCommandHandler : IRequestHandler<CreateSubscriptionCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IMediator _mediator; 

        public CreateSubscriptionCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(request.PlanId); 
            if (plan == null) throw new ApplicationException("Geçersiz Abonelik Planı.");
            
            var totalAmount = plan.Price * request.Quantity;

            
            // kartı saklamak için token oluşturma
            var permanentToken = await _paymentService.StoreCreditCardAsync(request.CustomerId.ToString(), request.CardToken);
            if (string.IsNullOrEmpty(permanentToken)) throw new ApplicationException("Kart saklama işlemi başarısız.");

            // ilk ödeme
            var (isSuccess, transactionId, errorMessage) = await _paymentService.ProcessPaymentAsync(totalAmount, "EUR", permanentToken);

            if (!isSuccess) throw new ApplicationException($"İlk ödeme başarısız oldu: {errorMessage}");
            
            var now = DateTime.UtcNow;
            var newSubscription = new Subscription
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                PlanId = request.PlanId,
                PlanPrice = plan.Price,
                PaymentToken = permanentToken, // kalıcı token
                Cycle = request.Cycle,
                StartDate = now,
                NextRenewalDate = now.AddMonths(1),
                Status = SubscriptionStatus.Active
            };
            await _unitOfWork.Subscriptions.AddAsync(newSubscription);
            
            //fatura ve ilk sipariş için tetikleme
            
            await _mediator.Send(new GenerateInvoiceCommand 
            {
                SubscriptionId = newSubscription.Id, Amount = totalAmount, PaymentTransactionId = transactionId 
            }, cancellationToken);

            await _mediator.Send(new CreateOrderCommand 
            {
                SubscriptionId = newSubscription.Id, CustomerId = request.CustomerId, 
                IsSubscriptionRenewal = false, ShippingAddress = request.ShippingAddress 
            }, cancellationToken);

            await _unitOfWork.CompleteAsync();
            return newSubscription.Id;
        }
    }
}