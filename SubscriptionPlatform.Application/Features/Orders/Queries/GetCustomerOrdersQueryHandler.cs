using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Application.Features.Orders.Queries
{
    public class GetCustomerOrdersQueryHandler : IRequestHandler<GetCustomerOrdersQuery, IReadOnlyList<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCustomerOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<OrderDto>> Handle(GetCustomerOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders.GetByCustomerIdAsync(request.CustomerId);

            if (orders == null)
            {
                return new List<OrderDto>();
            }

            return _mapper.Map<IReadOnlyList<OrderDto>>(orders);
        }
    }
}