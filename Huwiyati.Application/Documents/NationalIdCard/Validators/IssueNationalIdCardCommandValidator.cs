namespace Huwiyati.Application.Documents.NationalIdCard.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.NationalIdCard.Commands;

// Comprehensive validator defining complete validation rules for issuing a National ID Card command
public class IssueNationalIdCardCommandValidator : AbstractValidator<IssueNationalIdCardCommand>
{
    public IssueNationalIdCardCommandValidator()
    {
        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch identifier is required.");

        // If PersonId is NOT provided (Case B: New Person Creation), validate all Person fields thoroughly
        When(x => !x.PersonId.HasValue, () =>
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required when creating a new person.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

            RuleFor(x => x.FatherName)
                .NotEmpty().WithMessage("Father name is required when creating a new person.")
                .MaximumLength(50).WithMessage("Father name must not exceed 50 characters.");

            RuleFor(x => x.GrandfatherName)
                .NotEmpty().WithMessage("Grandfather name is required when creating a new person.")
                .MaximumLength(50).WithMessage("Grandfather name must not exceed 50 characters.");

            RuleFor(x => x.FamilyName)
                .NotEmpty().WithMessage("Family name is required when creating a new person.")
                .MaximumLength(50).WithMessage("Family name must not exceed 50 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotNull().WithMessage("Date of birth is required when creating a new person.")
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of birth must be in the past.");

            RuleFor(x => x.PlaceOfBirth)
                .NotEmpty().WithMessage("Place of birth is required when creating a new person.")
                .MaximumLength(100).WithMessage("Place of birth must not exceed 100 characters.");

            RuleFor(x => x.Gender)
                .NotNull().WithMessage("Gender is required when creating a new person.")
                .IsInEnum().WithMessage("Invalid gender value.");

            RuleFor(x => x.Nationality)
                .NotEmpty().WithMessage("Nationality is required when creating a new person.")
                .MaximumLength(50).WithMessage("Nationality must not exceed 50 characters.");

            RuleFor(x => x.MaritalStatus)
                .NotNull().WithMessage("Marital status is required when creating a new person.")
                .IsInEnum().WithMessage("Invalid marital status value.");

            RuleFor(x => x.Governorate)
                .NotEmpty().WithMessage("Governorate is required when creating a new person.")
                .MaximumLength(50).WithMessage("Governorate must not exceed 50 characters.");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("District is required when creating a new person.")
                .MaximumLength(50).WithMessage("District must not exceed 50 characters.");

            RuleFor(x => x.AddressDetails)
                .NotEmpty().WithMessage("Address details are required when creating a new person.")
                .MaximumLength(200).WithMessage("Address details must not exceed 200 characters.");

            RuleFor(x => x.PhotoUrl)
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.");

            RuleFor(x => x.BloodGroup)
                .NotNull().WithMessage("Blood group is required when creating a new person.")
                .IsInEnum().WithMessage("Invalid blood group value.");
        });

        // Issue date validation: if provided, must not be in the past
        When(x => x.IssueDate.HasValue, () =>
        {
            RuleFor(x => x.IssueDate!.Value)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Issue date cannot be in the past.");
        });
    }
}