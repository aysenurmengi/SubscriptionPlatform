using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Invoices.Commands
{
    public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenerateInvoiceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(GenerateInvoiceCommand request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            
            var newInvoice = new Invoice
            {
                Id = Guid.NewGuid(),
                SubscriptionId = request.SubscriptionId,
                Amount = request.Amount,
                IssueDate = now,
                DueDate = now.AddDays(7), // vade 7 gün
                PaymentStatus = PaymentStatus.Paid // ödeme başarılı paid
            };
            
            await _unitOfWork.Invoices.AddAsync(newInvoice);
            await _unitOfWork.CompleteAsync();

            return newInvoice.Id;
        }
    }
}