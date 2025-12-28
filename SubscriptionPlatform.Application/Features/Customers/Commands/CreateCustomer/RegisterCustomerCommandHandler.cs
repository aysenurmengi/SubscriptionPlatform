using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityService _identityService;

        public RegisterCustomerCommandHandler(IUnitOfWork unitOfWork, IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
        }

        public async Task<Guid> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var registrationResult = await _identityService.RegisterUserAsync(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Role);

            if (!registrationResult.IsSuccess)
            {
                throw new ApplicationException("Kullanıcı kaydı başarısız.");
            }

            if (request.Role == "Customer")
            {
                var newCustomer = new Customer
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                };

                await _unitOfWork.Customers.AddAsync(newCustomer);
                await _unitOfWork.CompleteAsync();

                return newCustomer.Id;
            }

            return Guid.Empty;
        }
    }
}