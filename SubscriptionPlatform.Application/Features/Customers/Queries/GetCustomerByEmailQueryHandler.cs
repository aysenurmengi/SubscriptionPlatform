using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Features.Queries;
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
                throw new ApplicationException("Müşteri bulunamadı.");
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }
    }
}