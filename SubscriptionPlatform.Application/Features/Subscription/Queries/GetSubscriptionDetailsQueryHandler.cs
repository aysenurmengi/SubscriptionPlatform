using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Queries
{
    public class GetSubscriptionDetailsQueryHandler : IRequestHandler<GetSubscriptionDetailsQuery, SubscriptionDetailDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSubscriptionDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SubscriptionDetailDto> Handle(GetSubscriptionDetailsQuery request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.SubscriptionId);

            if (subscription == null) throw new ApplicationException($"Abonelik bulunamadı: ID {request.SubscriptionId}");
            
            // plan adını göstermek için
            var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(subscription.PlanId);

            return new SubscriptionDetailDto
            {
                Id = subscription.Id,
                PlanName = plan?.Name ?? "Tanımlanmamış Plan",
                CurrentPrice = subscription.PlanPrice,
                Status = subscription.Status,
                NextRenewalDate = subscription.NextRenewalDate,
                Cycle = subscription.Cycle,
                StartDate = subscription.StartDate,
            };
        }
    }
}