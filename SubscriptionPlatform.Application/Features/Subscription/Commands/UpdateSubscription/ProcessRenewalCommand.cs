using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class ProcessRenewalCommand : IRequest<Unit>
    {
        public Guid SubscriptionId { get; set; }
    }
}