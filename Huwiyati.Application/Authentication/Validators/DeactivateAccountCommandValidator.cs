namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing business rules and required attributes on <see cref="DeactivateAccountCommand"/>.
/// </summary>
public class DeactivateAccountCommandValidator : AbstractValidator<DeactivateAccountCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeactivateAccountCommandValidator"/> class and configures validation rules.
    /// </summary>
    public DeactivateAccountCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("OTP Verification Code is required.");
    }
}
