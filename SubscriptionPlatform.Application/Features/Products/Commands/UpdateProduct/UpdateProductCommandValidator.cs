using FluentValidation;
using System;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEqual(Guid.Empty).WithMessage("Güncellenecek ürün ID'si zorunludur.");
            
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Ürün adı zorunludur.")
                .MaximumLength(100).WithMessage("Ürün adı 100 karakteri geçemez.");

            RuleFor(v => v.Price)
                .GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalıdır.");

            RuleFor(v => v.ImageUrl)
                .MaximumLength(255).WithMessage("Resim URL'i 255 karakterden uzun olamaz.");
        }
    }
}