using FluentValidation;

namespace SubscriptionPlatform.Application.Features.Subscriptions.Commands
{
    public class CreateSubscriptionCommandValidator : AbstractValidator<CreateSubscriptionCommand>
    {
        public CreateSubscriptionCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Müşteri bilgisi zorunludur.");
            RuleFor(x => x.PlanId).NotEmpty().WithMessage("Bir abonelik planı seçilmelidir.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Miktar en az 1 olmalıdır.");
            RuleFor(x => x.CardToken).NotEmpty().WithMessage("Ödeme yöntemi (kart) belirtilmelidir.");
            RuleFor(x => x.ShippingAddress).NotEmpty().WithMessage("Teslimat adresi boş geçilemez.");
            RuleFor(x => x.Cycle).IsInEnum().WithMessage("Geçersiz faturalama periyodu.");
        }
    }
}