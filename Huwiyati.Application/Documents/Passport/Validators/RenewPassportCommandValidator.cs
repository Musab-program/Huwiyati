namespace Huwiyati.Application.Documents.Passport.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.Passport.Commands;

/// <summary>
/// Validator defining validation rules for <see cref="RenewPassportCommand"/>.
/// </summary>
public class RenewPassportCommandValidator : AbstractValidator<RenewPassportCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenewPassportCommandValidator"/> class and configures validation rules.
    /// </summary>
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
