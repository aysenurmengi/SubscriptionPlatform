using MediatR;
using System;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class ProcessRenewalCommand : IRequest<bool>
    {
        public Guid SubscriptionId { get; set; }
    }
}