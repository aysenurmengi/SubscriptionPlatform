using FluentValidation;

namespace SubscriptionPlatform.Application.Features.Preferences.Commands
{
    public class UpdateCustomerPreferencesCommandValidator : AbstractValidator<UpdateCustomerPreferencesCommand>
    {
        public UpdateCustomerPreferencesCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleForEach(x => x.Preferences).ChildRules(p => {
                p.RuleFor(i => i.Key).NotEmpty().WithMessage("Tercih kategorisi boş olamaz (Örn: RoastLevel)");
                p.RuleFor(i => i.Value).NotEmpty().WithMessage("Tercih değeri boş olamaz (Örn: Dark)");
            });
        }
    }
}