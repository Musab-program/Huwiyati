namespace Huwiyati.Application.Family.Validators;

using FluentValidation;
using Huwiyati.Application.Family.Commands;

public class AddWifeCommandValidator : AbstractValidator<AddWifeCommand>
{
    public AddWifeCommandValidator()
    {
        RuleFor(x => x.HusbandNationalNumber)
            .NotEmpty().WithMessage("Husband National Number is required.")
            .Length(11).WithMessage("Husband National Number must be exactly 11 digits.");

        RuleFor(x => x.WifeNationalNumber)
            .NotEmpty().WithMessage("Wife National Number is required.")
            .Length(11).WithMessage("Wife National Number must be exactly 11 digits.")
            .Must((command, wifeNumber) => wifeNumber != command.HusbandNationalNumber)
            .WithMessage("Husband and Wife National Numbers cannot be identical.");

        RuleFor(x => x.MarriageContractNumber)
            .NotEmpty().WithMessage("Marriage contract number is required.")
            .MaximumLength(30).WithMessage("Marriage contract number must not exceed 30 characters.");

        RuleFor(x => x.MarriageDate)
            .NotNull().WithMessage("Marriage date is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Marriage date cannot be in the future.");

        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch identifier is required.");
    }
}
