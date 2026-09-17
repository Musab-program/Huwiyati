namespace Huwiyati.Application.Family.Validators;

using FluentValidation;
using Huwiyati.Application.Family.Commands;

// Validator defining validation rules for creating a new Family Card command
public class CreateFamilyCardCommandValidator : AbstractValidator<CreateFamilyCardCommand>
{
    public CreateFamilyCardCommandValidator()
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

        When(x => !string.IsNullOrWhiteSpace(x.ContractPhotoUrl), () =>
        {
            RuleFor(x => x.ContractPhotoUrl)
                .MaximumLength(500).WithMessage("Contract photo URL must not exceed 500 characters.");
        });
    }
}
