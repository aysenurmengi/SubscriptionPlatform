using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using PlanEntity = SubscriptionPlatform.Domain.Entities.SubscriptionPlan;

namespace SubscriptionPlatform.Application.Features.SubscriptionPlan.Commands
{
    public class CreateSubscriptionPlanCommandHandler : IRequestHandler<CreateSubscriptionPlanCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubscriptionPlanCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateSubscriptionPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = new PlanEntity
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                IsActive = true,
                PlanItems = request.Items.Select(i => new SubscriptionPlanItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            await _unitOfWork.SubscriptionPlans.AddAsync(plan);
            await _unitOfWork.CompleteAsync();

            return plan.Id;
        }
    }
}