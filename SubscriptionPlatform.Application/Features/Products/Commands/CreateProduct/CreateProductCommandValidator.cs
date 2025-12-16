using FluentValidation;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Ürün adı zorunludur.")
                .MaximumLength(100).WithMessage("Ürün adı 100 karakteri geçemez.");

            RuleFor(v => v.Price)
                .GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalıdır.");

            RuleFor(v => v.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakteri geçemez.");
        }
    }
}