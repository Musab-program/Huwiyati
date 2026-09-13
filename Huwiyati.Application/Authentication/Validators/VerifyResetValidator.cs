namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

public class VerifyResetValidator : AbstractValidator<VerifyResetCommand>
{
    public VerifyResetValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Code must be 6 digits.");
    }
}