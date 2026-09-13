namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        // 1. National Number validation
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.")
            .Matches(@"^\d+$").WithMessage("National Number must contain digits only.");

        // 2. Date of Birth validation
        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of Birth must be in the past.");

        // 3. Phone Number validation
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required.")
            .Matches(@"^(\+?967|00967)?\s*7[\d\s-]{8,10}$").WithMessage("Invalid Phone Number format.");

        // 4. Email validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        // 5. Password complexity validation
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.");
    }
}
