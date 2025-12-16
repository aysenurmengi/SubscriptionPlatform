using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using static SubscriptionPlatform.Application.Features.Customers.Queries.GetCustomerSubscriptionsQuery;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public class GetCustomerSubscriptionsHandler : IRequestHandler<GetCustomerSubscriptionsQuery, IReadOnlyList<SubscriptionDto>>
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCustomerSubscriptionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<IReadOnlyList<SubscriptionDto>> Handle(GetCustomerSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);
            
            if (customer == null)
            {
                return new List<SubscriptionDto>();
            }

            var customerSubscriptions = await _unitOfWork.Subscriptions.GetByCustomerIdAsync(request.CustomerId);
            var subscriptionDtos = _mapper.Map<IReadOnlyList<SubscriptionDto>>(customerSubscriptions);

            return subscriptionDtos;
        }
    }
}