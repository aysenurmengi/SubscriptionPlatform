using MediatR;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = request.CustomerId,
                SubscriptionId = request.SubscriptionId,
                ShippingAddress = request.ShippingAddress,
                IsSubscriptionRenewal = request.IsSubscriptionRenewal,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Created,
                ShippingStatus = ShippingStatus.AwaitingFulfillment,
                TrackingNumber = string.Empty,
                TotalAmount = 0 
            };

            decimal calculatedTotal = 0;

            if (request.Items != null && request.Items.Any())
            {
                foreach (var itemDto in request.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);

                    if (product != null)
                    {
                        var orderItem = new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = newOrder.Id,
                            ProductId = product.Id,
                            ProductName = product.Name,
                            UnitPrice = product.Price,
                            Quantity = itemDto.Quantity
                        };

                        newOrder.OrderItems.Add(orderItem);
                        calculatedTotal += orderItem.UnitPrice * orderItem.Quantity;
                    }
                }
            }

            // abonelik siparişini işle (eğer SubscriptionId varsa)
            if (request.SubscriptionId.HasValue) //fiyat 0 görünmesin diye
            {   var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.SubscriptionId.Value);
                if (subscription != null && !request.Items.Any())
                {
                    newOrder.TotalAmount = subscription.PlanPrice; 
                }
            }

            newOrder.TotalAmount = calculatedTotal;

            await _unitOfWork.Orders.AddAsync(newOrder);
            await _unitOfWork.CompleteAsync();

            return newOrder.Id;
        }
    }
}