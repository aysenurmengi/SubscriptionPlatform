using FluentValidation;
using System;

namespace SubscriptionPlatform.Application.Features.Inventory.Commands
{
    public class UpdateInventoryQuantityCommandValidator : AbstractValidator<UpdateInventoryQuantityCommand>
    {
        public UpdateInventoryQuantityCommandValidator()
        {
            RuleFor(v => v.ProductId)
                .NotEqual(Guid.Empty).WithMessage("Ürün ID'si zorunludur.");
            
            RuleFor(v => v.NewStockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stok miktarı negatif olamaz.");

            RuleFor(v => v.Reason)
                .NotEmpty().WithMessage("Güncelleme nedeni belirtilmelidir.");
        }
    }
}