using AutoMapper;
using SubscriptionPlatform.Application.DTOs.Customers;
using SubscriptionPlatform.Application.DTOs.Products;
using SubscriptionPlatform.Application.DTOs.Subscription.Requests;
using SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer;
using SubscriptionPlatform.Application.Features.Customers.Commands.UpdateCustomer;
using SubscriptionPlatform.Application.Features.Inventory.Queries;
using SubscriptionPlatform.Application.Features.Invoices.Queries;
using SubscriptionPlatform.Application.Features.Orders.Queries;
using SubscriptionPlatform.Application.Features.Products.Commands;
using SubscriptionPlatform.Application.Features.Subscriptions.Commands;
using SubscriptionPlatform.Domain.Entities;

namespace SubscriptionPlatform.Application.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Inventory, InventoryStatusDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.OrderDate));
            CreateMap<CreateCustomerRequest, RegisterCustomerCommand>();
            CreateMap<UpdateCustomerRequest, UpdateCustomerProfileCommand>();
            CreateMap<CreateSubscriptionRequest, CreateSubscriptionCommand>();
            CreateMap<CreateProductRequest, CreateProductCommand>();
            CreateMap<UpdateProductRequest, UpdateProductCommand>();
        }
    }
}