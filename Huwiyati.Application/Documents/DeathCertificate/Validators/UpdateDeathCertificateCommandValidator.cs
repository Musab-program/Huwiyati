namespace Huwiyati.Application.Documents.DeathCertificate.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.DeathCertificate.Commands;

/// <summary>
/// Validator for validating properties and business rules of <see cref="UpdateDeathCertificateCommand"/>.
/// </summary>
public class UpdateDeathCertificateCommandValidator : AbstractValidator<UpdateDeathCertificateCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateDeathCertificateCommandValidator"/> class and configures validation rules.
    /// </summary>
    public UpdateDeathCertificateCommandValidator()
    {
        RuleFor(x => x.DeathCertificateId)
            .NotEmpty().WithMessage("Death Certificate ID is required.");

        RuleFor(x => x.DeathDate)
            .NotEmpty().WithMessage("Date of death is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of death cannot be in the future.");

        RuleFor(x => x.PlaceOfDeath)
            .NotEmpty().WithMessage("Place of death is required.")
            .MaximumLength(200).WithMessage("Place of death cannot exceed 200 characters.");

        RuleFor(x => x.CauseOfDeath)
            .NotEmpty().WithMessage("Cause of death is required.")
            .MaximumLength(500).WithMessage("Cause of death cannot exceed 500 characters.");
    }
}
