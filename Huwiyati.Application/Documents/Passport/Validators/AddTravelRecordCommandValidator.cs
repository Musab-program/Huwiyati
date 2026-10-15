namespace Huwiyati.Application.Documents.Passport.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.Passport.Commands;

/// <summary>
/// Validator defining validation rules for <see cref="AddTravelRecordCommand"/>.
/// </summary>
public class AddTravelRecordCommandValidator : AbstractValidator<AddTravelRecordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddTravelRecordCommandValidator"/> class and configures validation rules.
    /// </summary>
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
