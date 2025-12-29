using MediatR;
using SubscriptionPlatform.Application.Common.Exceptions;
using SubscriptionPlatform.Application.Interfaces;
using SubscriptionPlatform.Application.Interfaces.Repositories;
using SubscriptionPlatform.Domain.Entities;
using SubscriptionPlatform.Domain.Enums;

namespace SubscriptionPlatform.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            bool isSubscriptionRenewalOrder = request.SubscriptionId.HasValue && request.IsSubscriptionRenewal;
            bool isSingleOrder = !request.SubscriptionId.HasValue && !request.IsSubscriptionRenewal;

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

                    if (product == null)
                        throw new NotFoundException(nameof(Product), itemDto.ProductId);

                    var orderItem = new OrderItem
                    {
                        Id = Guid.NewGuid(),
                        OrderId = newOrder.Id,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        UnitPrice = product.Price,
                        Quantity = itemDto.Quantity
                    };

                    if (isSingleOrder || isSubscriptionRenewalOrder)
                    {
                        var inventory = await _unitOfWork.Inventories.GetByProductIdAsync(product.Id);

                        if (inventory == null)
                            throw new NotFoundException("Inventory", product.Id);

                        if (inventory.StockQuantity < itemDto.Quantity)
                            throw new ApplicationException($"{product.Name} için yeterli stok yok");

                        inventory.StockQuantity -= itemDto.Quantity;
                        _unitOfWork.Inventories.UpdateAsync(inventory);
                    }

                    newOrder.OrderItems.Add(orderItem);
                    calculatedTotal += orderItem.UnitPrice * orderItem.Quantity;
                }
            }



            // abonelik siparişini işle (eğer SubscriptionId varsa)
            if (request.SubscriptionId.HasValue && !request.Items.Any())
            {
                var subscription = await _unitOfWork.Subscriptions.GetByIdAsync(request.SubscriptionId.Value);
                if (subscription == null)
                    throw new NotFoundException(nameof(Subscription), request.SubscriptionId.Value);

                newOrder.TotalAmount = subscription.PlanPrice;
            }
            else
            {
                // ürün varsa veya abonelik değilse hesaplanan tutarı al
                newOrder.TotalAmount = calculatedTotal;
            }

            await _unitOfWork.Orders.AddAsync(newOrder);
            await _unitOfWork.CompleteAsync();

            //mail
            var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);

            if (customer != null)
            {
                bool sendMail = isSingleOrder || isSubscriptionRenewalOrder;

                string subject;
                string body;

                if (isSubscriptionRenewalOrder)
                {
                    subject = "Aboneliğiniz Yenilendi 🎉";

                    body = $@"
                        <div style='font-family: Arial'>
                            <h2>Aboneliğiniz başarıyla yenilendi 🔄</h2>
                            <p>Merhaba <b>{customer.FirstName}</b>,</p>

                            <p>Aboneliğiniz kapsamında yeni siparişiniz oluşturuldu.</p>

                            <p>
                                <b>Sipariş No:</b> {newOrder.Id}<br/>
                                <b>Tutar:</b> {newOrder.TotalAmount} ₺
                            </p>

                            <p>
                                Bir sonraki yenileme tarihinde otomatik olarak
                                siparişiniz oluşturulacaktır.
                            </p>

                            <hr />
                            <small>Bu mail bilgilendirme amaçlıdır.</small>
                        </div>";
                }
                else
                {
                    subject = "Siparişiniz Alındı 🛒";

                    body = $@"
                        <div style='font-family: Arial'>
                            <h2>Siparişiniz başarıyla oluşturuldu 🎉</h2>
                            <p>Merhaba <b>{customer.FirstName}</b>,</p>

                            <p>Siparişiniz sistemimize alınmıştır.</p>

                            <p>
                                <b>Sipariş No:</b> {newOrder.Id}<br/>
                                <b>Tutar:</b> {newOrder.TotalAmount} ₺
                            </p>

                            <p>Siparişiniz hazırlanıp kargoya verildiğinde bilgilendirileceksiniz.</p>

                            <hr />
                            <small>Bizi tercih ettiğiniz için teşekkür ederiz.</small>
                        </div>";
                }

                await _emailService.SendEmailAsync(
                    customer.Email,
                    subject,
                    body
                );
            }


            return newOrder.Id;
        }
    }
}