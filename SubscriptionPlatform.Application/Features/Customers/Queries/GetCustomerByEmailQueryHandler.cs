using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Customers.Queries
{
    public class GetCustomerByEmailQueryHandler : IRequestHandler<GetCustomerByEmailQuery, CustomerDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCustomerByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerDto> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByEmailAsync(request.Email);
            if (customer == null)
            {
                throw new ApplicationException("Müşteri bulunamadı.");
            }

            var customerDto = new CustomerDto
            {
                Id = customer.Id,
                Email = customer.Email,
                FullName = $"{customer.FirstName} {customer.LastName}"
            };
            return customerDto;
        }

        
    }
}