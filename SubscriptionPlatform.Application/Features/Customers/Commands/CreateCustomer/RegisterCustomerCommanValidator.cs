using FluentValidation;
using SubscriptionPlatform.Application.Features.Customers.Commands.CreateCustomer;

namespace SubscriptionPlatform.Application.Features.Customers.Commands
{
    // Validator, hangi Command'ı doğrulayacağını belirtir.
    public class RegisterCustomerCommandValidator : AbstractValidator<RegisterCustomerCommand>
    {
        public RegisterCustomerCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("İsim alanı zorunludur.")
                .MaximumLength(50).WithMessage("İsim 50 karakterden uzun olamaz.");

            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Soyisim alanı zorunludur.");
                
            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("Email adresi zorunludur.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Şifre alanı zorunludur.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.");
        }
    }
}