using FluentValidation;

namespace SubscriptionPlatform.Application.Features.Products.Commands
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEqual(Guid.Empty).WithMessage("Silinecek ürün ID'si zorunludur.");
        }
    }
}