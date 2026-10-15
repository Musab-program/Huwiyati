namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing required fields and code length on <see cref="VerifyResetCommand"/>.
/// </summary>
public class VerifyResetValidator : AbstractValidator<VerifyResetCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyResetValidator"/> class and configures validation rules.
    /// </summary>
    public VerifyResetValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Code must be 6 digits.");
    }
}