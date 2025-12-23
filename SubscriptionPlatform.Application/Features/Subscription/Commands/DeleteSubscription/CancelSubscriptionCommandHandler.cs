using MediatR;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
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

            if (subscription == null) throw new NotFoundException(nameof(Subscription), request.SubscriptionId);

            if (request.CancelImmediately)
            {
                //anında iptal
                subscription.Status = SubscriptionStatus.Cancelled;
                
            }
            else
            {
                subscription.Status = SubscriptionStatus.Cancelled; 
            }
            

            await _unitOfWork.Subscriptions.UpdateAsync(subscription);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}