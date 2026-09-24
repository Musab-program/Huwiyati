namespace Huwiyati.Application.Documents.Passport.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.Passport.Commands;

// Validator defining FluentValidation rules for RenewPassportCommand
public class RenewPassportCommandValidator : AbstractValidator<RenewPassportCommand>
{
    public RenewPassportCommandValidator()
    {
        RuleFor(v => v.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(v => v.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch ID is required.");

        When(v => v.PassportType.HasValue, () =>
        {
            RuleFor(v => v.PassportType!.Value)
                .IsInEnum().WithMessage("Invalid passport type specified.");
        });
    }
}
