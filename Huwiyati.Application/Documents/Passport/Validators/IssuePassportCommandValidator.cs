namespace Huwiyati.Application.Documents.Passport.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.Passport.Commands;

/// <summary>
/// Validator defining validation rules for <see cref="IssuePassportCommand"/>.
/// </summary>
public class IssuePassportCommandValidator : AbstractValidator<IssuePassportCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IssuePassportCommandValidator"/> class and configures validation rules.
    /// </summary>
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
