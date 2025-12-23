using MediatR;
using SubscriptionPlatform.Application.Features.Preferences.Commands;
using SubscriptionPlatform.Application.Interfaces.Repositories;

namespace SubscriptionPlatform.Application.Features.Preferences.Queries
{
    public class GetCustomerPreferencesQueryHandler : IRequestHandler<GetCustomerPreferencesQuery, List<PreferenceItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCustomerPreferencesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PreferenceItemDto>> Handle(GetCustomerPreferencesQuery request, CancellationToken cancellationToken)
        {
            var preferences = await _unitOfWork.Preferences.GetAllByCustomerIdAsync(request.CustomerId);

            return preferences.Select(p => new PreferenceItemDto
            {
                Key = p.Key,
                Value = p.Value
            }).ToList();
        }
    }
}