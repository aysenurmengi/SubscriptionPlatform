using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCustomerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
            if (customer == null)
            {
                throw new ApplicationException("Müşteri bulunamadı.");
            }

            await _unitOfWork.Customers.DeleteAsync(customer);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
    
}