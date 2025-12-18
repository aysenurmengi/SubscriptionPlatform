using AutoMapper;
using SubscriptionPlatform.Application.Features.Inventory.Queries;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Inventory, InventoryStatusDto>();
        }
        
        
    }
}