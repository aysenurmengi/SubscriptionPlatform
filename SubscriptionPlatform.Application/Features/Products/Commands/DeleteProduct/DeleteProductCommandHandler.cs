using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
            {
                return Unit.Value; 
            }
            await _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}