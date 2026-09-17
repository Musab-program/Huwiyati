namespace Huwiyati.Application.Documents.NationalIdCard.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.NationalIdCard.Commands;

// Validator defining strict required validation rules for updating citizen person details
public class UpdatePersonDataCommandValidator : AbstractValidator<UpdatePersonDataCommand>
{
    public UpdatePersonDataCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.")
            .Length(11).WithMessage("National Number must be exactly 11 digits.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.FatherName)
            .NotEmpty().WithMessage("Father name is required.")
            .MaximumLength(50).WithMessage("Father name must not exceed 50 characters.");

        RuleFor(x => x.GrandfatherName)
            .NotEmpty().WithMessage("Grandfather name is required.")
            .MaximumLength(50).WithMessage("Grandfather name must not exceed 50 characters.");

        RuleFor(x => x.FamilyName)
            .NotEmpty().WithMessage("Family name is required.")
            .MaximumLength(50).WithMessage("Family name must not exceed 50 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotNull().WithMessage("Date of birth is required.")
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.PlaceOfBirth)
            .NotEmpty().WithMessage("Place of birth is required.")
            .MaximumLength(100).WithMessage("Place of birth must not exceed 100 characters.");

        RuleFor(x => x.MaritalStatus)
            .NotNull().WithMessage("Marital status is required.")
            .IsInEnum().WithMessage("Invalid marital status value.");

        RuleFor(x => x.Governorate)
            .NotEmpty().WithMessage("Governorate is required.")
            .MaximumLength(50).WithMessage("Governorate must not exceed 50 characters.");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required.")
            .MaximumLength(50).WithMessage("District must not exceed 50 characters.");

        RuleFor(x => x.AddressDetails)
            .NotEmpty().WithMessage("Address details are required.")
            .MaximumLength(200).WithMessage("Address details must not exceed 200 characters.");

        When(x => !string.IsNullOrWhiteSpace(x.PhotoUrl), () =>
        {
            RuleFor(x => x.PhotoUrl)
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.");
        });

        RuleFor(x => x.BloodGroup)
            .NotNull().WithMessage("Blood group is required.")
            .IsInEnum().WithMessage("Invalid blood group value.");
    }
}
