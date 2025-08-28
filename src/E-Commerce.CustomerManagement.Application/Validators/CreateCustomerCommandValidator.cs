using E_Commerce.CustomerManagement.Application.Commands;
using FluentValidation;

namespace E_Commerce.CustomerManagement.Application.Validators;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Valid email address is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(1, 100)
            .WithMessage("First name must be between 1 and 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(1, 100)
            .WithMessage("Last name must be between 1 and 100 characters");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be in the past");
    }
}
