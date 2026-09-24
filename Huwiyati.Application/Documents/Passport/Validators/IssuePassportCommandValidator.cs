namespace Huwiyati.Application.Documents.Passport.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.Passport.Commands;

// Validator defining FluentValidation rules for IssuePassportCommand
public class IssuePassportCommandValidator : AbstractValidator<IssuePassportCommand>
{
    public IssuePassportCommandValidator()
    {
        // Personal Information Validation
        RuleFor(v => v.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        // Issuing Branch Validation
        RuleFor(v => v.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch ID is required.");

        // Passport Type Validation
        RuleFor(v => v.PassportType)
            .IsInEnum().WithMessage("Invalid passport type specified.");
    }
}
