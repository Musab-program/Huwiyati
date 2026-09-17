namespace Huwiyati.Application.Family.Validators;

using FluentValidation;
using Huwiyati.Application.Family.Commands;

public class RenewFamilyCardCommandValidator : AbstractValidator<RenewFamilyCardCommand>
{
    public RenewFamilyCardCommandValidator()
    {
        RuleFor(x => x.FamilyNumber)
            .NotEmpty().WithMessage("Family number is required.")
            .Length(11).WithMessage("Family number must be exactly 11 digits.");

        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch identifier is required.");
    }
}