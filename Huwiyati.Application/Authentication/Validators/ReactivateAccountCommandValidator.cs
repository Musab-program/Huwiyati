namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing business rules and required attributes on <see cref="ReactivateAccountCommand"/>.
/// </summary>
public class ReactivateAccountCommandValidator : AbstractValidator<ReactivateAccountCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReactivateAccountCommandValidator"/> class and configures validation rules.
    /// </summary>
    public ReactivateAccountCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("OTP Verification Code is required.");
    }
}
