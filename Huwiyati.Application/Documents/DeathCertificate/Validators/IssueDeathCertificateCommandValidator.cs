namespace Huwiyati.Application.Documents.DeathCertificate.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.DeathCertificate.Commands;

public class IssueDeathCertificateCommandValidator : AbstractValidator<IssueDeathCertificateCommand>
{
    public IssueDeathCertificateCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("Deceased person national number is required.")
            .Length(11).WithMessage("Deceased person national number must be exactly 11 digits.");

        RuleFor(x => x.HospitalBranchId)
            .NotEmpty().WithMessage("Hospital branch ID is required.");

        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing civil registry branch ID is required.");

        RuleFor(x => x.DeathDate)
            .NotEmpty().WithMessage("Date of death is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of death cannot be in the future.");

        RuleFor(x => x.PlaceOfDeath)
            .MaximumLength(200).WithMessage("Place of death cannot exceed 200 characters.");

        RuleFor(x => x.CauseOfDeath)
            .MaximumLength(500).WithMessage("Cause of death cannot exceed 500 characters.");
    }
}
