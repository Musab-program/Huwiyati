namespace Huwiyati.Application.Documents.Passport.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.Passport.Commands;

// Validator defining FluentValidation rules for AddTravelRecordCommand
public class AddTravelRecordCommandValidator : AbstractValidator<AddTravelRecordCommand>
{
    public AddTravelRecordCommandValidator()
    {
        RuleFor(v => v.PassportNumber)
            .NotEmpty().WithMessage("Passport Number is required.");

        RuleFor(v => v.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch ID is required.");

        RuleFor(v => v.Country)
            .NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(50).WithMessage("Country name must not exceed 50 characters.");

        RuleFor(v => v.EntryDate)
            .NotEmpty().WithMessage("Entry date is required.");

        When(v => v.ExitDate.HasValue, () =>
        {
            RuleFor(v => v.ExitDate!.Value)
                .GreaterThanOrEqualTo(v => v.EntryDate)
                .WithMessage("Exit date must be equal to or after entry date.");
        });
    }
}
