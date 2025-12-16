using MediatR;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class CancelSubscriptionCommand : IRequest<Unit>
    {
        public Guid SubscriptionId { get; set; }
        public bool CancelImmediately { get; set; } = false; 
    }
}