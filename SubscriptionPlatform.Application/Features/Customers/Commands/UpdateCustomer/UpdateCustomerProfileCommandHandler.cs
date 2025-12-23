using MediatR;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerProfileCommandHandler : IRequestHandler<UpdateCustomerProfileCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCustomerProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            //mail benzersiz mi?
            if (customer.Email != request.Email)
            {
                var existingCustomer = await _unitOfWork.Customers.GetByEmailAsync(request.Email);
                
                if (existingCustomer != null && existingCustomer.Id != customer.Id)
                {
                    throw new ApplicationException("Bu email adresi zaten başka bir kullanıcı tarafından kullanılmaktadır.");
                }
                
                customer.Email = request.Email;
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;

            _unitOfWork.Customers.UpdateAsync(customer);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}