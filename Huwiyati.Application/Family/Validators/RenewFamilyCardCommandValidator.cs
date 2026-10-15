namespace Huwiyati.Application.Family.Validators;

using FluentValidation;
using Huwiyati.Application.Family.Commands;

/// <summary>
/// Validator defining validation rules for <see cref="RenewFamilyCardCommand"/>.
/// </summary>
public class RenewFamilyCardCommandValidator : AbstractValidator<RenewFamilyCardCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenewFamilyCardCommandValidator"/> class and configures validation rules.
    /// </summary>
    public RenewFamilyCardCommandValidator()
    {
        RuleFor(x => x.FamilyNumber)
            .NotEmpty().WithMessage("Family number is required.")
            .Length(11).WithMessage("Family number must be exactly 11 digits.");

        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch identifier is required.");
    }
}