using MediatR;
using System;
using System.Collections.Generic;

namespace SubscriptionPlatform.Application.Features.Preferences.Commands
{
    public class PreferenceItemDto 
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class UpdateCustomerPreferencesCommand : IRequest<Unit>
    {
        public Guid CustomerId { get; set; }
        public List<PreferenceItemDto> Preferences { get; set; }
    }
}