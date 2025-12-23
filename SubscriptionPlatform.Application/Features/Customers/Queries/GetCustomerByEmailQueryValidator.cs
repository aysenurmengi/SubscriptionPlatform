using FluentValidation;
using SubscriptionPlatform.Application.Features.Customers.Queries;

public class GetCustomerByEmailQueryValidator : AbstractValidator<GetCustomerByEmailQuery>
{
    public GetCustomerByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}