using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelSubscriptionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.SubscriptionId);

            if (subscription == null) return Unit.Value;

            if (request.CancelImmediately)
            {
                //anında iptal
                subscription.Status = SubscriptionStatus.Cancelled;
                
            }
            else
            {
                subscription.Status = SubscriptionStatus.Cancelled; 
            }
            

            _unitOfWork.Subscriptions.UpdateAsync(subscription);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}