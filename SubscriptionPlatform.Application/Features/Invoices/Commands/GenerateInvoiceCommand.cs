using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Invoices.Commands
{
    // Oluşturulan Faturanın ID'sini döndürür
    public class GenerateInvoiceCommand : IRequest<Guid> 
    {
        public Guid SubscriptionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentTransactionId { get; set; } // ödeme Servisinden gelen referans ID'si
    }
}