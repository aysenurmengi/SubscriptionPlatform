using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Queries
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCustomerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request,CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
            if (customer == null)
            {
                throw new ApplicationException("Müşteri bulunamadı.");
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return customerDto;
        }

    }
}