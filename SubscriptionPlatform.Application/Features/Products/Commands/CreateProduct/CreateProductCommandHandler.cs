using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive 
            };

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            return product.Id;
        }
    }
}