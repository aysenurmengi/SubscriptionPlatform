using MediatR;
using SubscriptionPlatform.Domain.Enums;
using System;
using System.Collections.Generic;

namespace SubscriptionPlatform.Application.Features.Invoices.Queries
{
    // blirli bir aboneliğe ait tüm faturalar 
    public class GetSubscriptionInvoicesQuery : IRequest<IReadOnlyList<InvoiceDto>>
    {
        public Guid SubscriptionId { get; set; }
    }

    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}