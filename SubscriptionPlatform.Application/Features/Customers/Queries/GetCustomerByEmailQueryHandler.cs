using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCustomerByEmailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByEmailAsync(request.Email);
            if (customer == null)
            {
                throw new NotFoundException("Customer", request.Email);
            }

            return _mapper.Map<CustomerDto>(customer);
        }

        
    }
}