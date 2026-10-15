namespace Huwiyati.Application.Documents.BirthCertificate.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.BirthCertificate.Commands;

/// <summary>
/// Validator for validating properties and rules of <see cref="UpdateChildDataCommand"/>.
/// </summary>
public class UpdateChildDataCommandValidator : AbstractValidator<UpdateChildDataCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateChildDataCommandValidator"/> class and configures validation rules.
    /// </summary>
    public UpdateChildDataCommandValidator()
    {
        RuleFor(x => x.BirthCertificateId)
            .NotEmpty().WithMessage("Birth certificate ID is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Date of birth cannot be in the future.");

        RuleFor(x => x.PlaceOfBirth)
            .NotEmpty().WithMessage("Place of birth is required.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender value.");

        RuleFor(x => x.BloodGroup)
            .IsInEnum().WithMessage("Invalid blood group value.");

        RuleFor(x => x.Governorate)
            .NotEmpty().WithMessage("Governorate is required.");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required.");

        RuleFor(x => x.AddressDetails)
            .NotEmpty().WithMessage("Address details are required.");
    }
}
