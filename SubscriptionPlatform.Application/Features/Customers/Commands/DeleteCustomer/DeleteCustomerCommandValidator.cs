using FluentValidation;
using SubscriptionPlatform.Application.Features.Customers.Commands.DeleteCustomer;
namespace SubscriptionPlatform.Application.Features.Customers.Commands
{
    public class DeleteCustomerCommandValidator : AbstractValidator<DeleteCustomerCommand>
    {
        public DeleteCustomerCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEqual(Guid.Empty).WithMessage("Silinecek müşteri ID'si zorunludur.");
        }
    }
}