using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using static SubscriptionPlatform.Application.Features.Customers.Queries.GetCustomerSubscriptionsQuery;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public class GetCustomerSubscriptionsHandler : IRequestHandler<GetCustomerSubscriptionsQuery, IReadOnlyList<SubscriptionDto>>
    {
        public readonly IUnitOfWork _unitOfWork;
        public GetCustomerSubscriptionsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
       public async Task<IReadOnlyList<SubscriptionDto>> Handle(GetCustomerSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
            
            if (customer == null)
            {
                return new List<SubscriptionDto>();
            }

            var subscriptions = await _unitOfWork.Subscriptions.GetByCustomerIdAsync(request.CustomerId);
            
            var dtoList = new List<SubscriptionDto>();

            foreach (var sub in subscriptions)
            {
                var plan = await _unitOfWork.SubscriptionPlans.GetByIdAsync(sub.PlanId);

                string realPlanName = plan != null ? plan.Name : "Bilinmeyen Plan";

                dtoList.Add(new SubscriptionDto
                {
                    SubscriptionId = sub.Id,
                    PlanName = realPlanName,  
                    Price = sub.PlanPrice,    
                    BillingCycle = sub.Cycle,  
                    Status = sub.Status,    
                    NextRenewalDate = sub.NextRenewalDate
                });
            }

            return dtoList;
        }
    }
}