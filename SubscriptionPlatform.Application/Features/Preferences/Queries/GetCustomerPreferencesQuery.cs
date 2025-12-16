using MediatR;
using SubscriptionPlatform.Application.Features.Preferences.Commands;
using System;
using System.Collections.Generic;

namespace SubscriptionPlatform.Application.Features.Preferences.Queries
{
    public class GetCustomerPreferencesQuery : IRequest<List<PreferenceItemDto>>
    {
        public Guid CustomerId { get; set; }
    }
}