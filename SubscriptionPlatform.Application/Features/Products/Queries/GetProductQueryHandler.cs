using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Application.Features.Products.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new ApplicationException($"Ürün bulunamadı: ID {request.Id}");
            }

            return _mapper.Map<ProductDto>(product);
        }
    }
}