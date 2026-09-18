namespace Huwiyati.Application.Documents.BirthCertificate.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.BirthCertificate.Commands;

public class IssueBirthCertificateCommandValidator : AbstractValidator<IssueBirthCertificateCommand>
{
    public IssueBirthCertificateCommandValidator()
    {
        RuleFor(x => x.FatherNationalNumber)
            .NotEmpty().WithMessage("Father national number is required.")
            .Length(11).WithMessage("Father national number must be exactly 11 digits.");

        RuleFor(x => x.MotherNationalNumber)
            .NotEmpty().WithMessage("Mother national number is required.")
            .Length(11).WithMessage("Mother national number must be exactly 11 digits.")
            .NotEqual(x => x.FatherNationalNumber).WithMessage("Mother national number cannot be the same as father national number.");

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

        RuleFor(x => x.HospitalBranchId)
            .NotEmpty().WithMessage("Hospital branch ID is required.");

        RuleFor(x => x.FamilyId)
            .NotEmpty().WithMessage("Family ID is required.");

        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch ID is required.");
    }
}
