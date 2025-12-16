using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new ApplicationException($"Güncellenecek ürün bulunamadı: ID {request.Id}");
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ImageUrl = request.ImageUrl;
            product.IsActive = request.IsActive;

            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}