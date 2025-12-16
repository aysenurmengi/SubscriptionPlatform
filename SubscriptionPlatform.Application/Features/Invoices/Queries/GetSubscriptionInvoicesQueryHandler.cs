using AutoMapper;
using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SubscriptionPlatform.Application.Features.Invoices.Queries
{
    public class GetSubscriptionInvoicesQueryHandler : IRequestHandler<GetSubscriptionInvoicesQuery, IReadOnlyList<InvoiceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSubscriptionInvoicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<InvoiceDto>> Handle(GetSubscriptionInvoicesQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _unitOfWork.Invoices.GetBySubscriptionIdAsync(request.SubscriptionId);

            if (invoices == null)
            {
                return new List<InvoiceDto>();
            }

            return _mapper.Map<IReadOnlyList<InvoiceDto>>(invoices);
        }
    }
}