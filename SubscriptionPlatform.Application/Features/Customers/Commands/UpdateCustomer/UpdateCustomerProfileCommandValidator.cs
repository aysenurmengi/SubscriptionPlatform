using FluentValidation;
using SubscriptionPlatform.Application.Features.Customers.Commands.UpdateCustomer;

namespace SubscriptionPlatform.Application.Features.Customers.Commands
{
    public class UpdateCustomerProfileCommandValidator : AbstractValidator<UpdateCustomerProfileCommand>
    {
        public UpdateCustomerProfileCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEqual(Guid.Empty).WithMessage("Müşteri ID'si zorunludur.");
            
            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("İsim zorunludur.")
                .MaximumLength(50).WithMessage("İsim 50 karakterden uzun olamaz.");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Soyisim zorunludur.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email adresi zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
        }
    }
}