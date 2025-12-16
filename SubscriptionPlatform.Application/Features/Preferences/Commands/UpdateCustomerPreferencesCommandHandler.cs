using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SubscriptionPlatform.Application.Features.Preferences.Commands
{
    public class UpdateCustomerPreferencesCommandHandler : IRequestHandler<UpdateCustomerPreferencesCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCustomerPreferencesCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCustomerPreferencesCommand request, CancellationToken cancellationToken)
        {
            var existingPreferences = await _unitOfWork.Preferences.GetAllByCustomerIdAsync(request.CustomerId);

            if (existingPreferences.Any())
            {
                await _unitOfWork.Preferences.DeleteRangeAsync(existingPreferences); 
            }

            var newPreferences = request.Preferences.Select(item => new Preference
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                Key = item.Key,
                Value = item.Value
            }).ToList();

            await _unitOfWork.Preferences.AddRangeAsync(newPreferences);
            
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}