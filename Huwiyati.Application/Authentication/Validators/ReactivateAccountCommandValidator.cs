namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

public class ReactivateAccountCommandValidator : AbstractValidator<ReactivateAccountCommand>
{
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
