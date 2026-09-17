namespace Huwiyati.Application.Documents.NationalIdCard.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.NationalIdCard.Commands;

// Validator defining comprehensive validation rules for renewing a National ID Card command
public class RenewNationalIdCardCommandValidator : AbstractValidator<RenewNationalIdCardCommand>
{
    public RenewNationalIdCardCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.")
            .Length(11).WithMessage("National Number must be exactly 11 digits.");

        RuleFor(x => x.IssuingBranchId)
            .NotEmpty().WithMessage("Issuing branch identifier is required.");

        When(x => x.MaritalStatus.HasValue, () =>
        {
            RuleFor(x => x.MaritalStatus!.Value)
                .IsInEnum().WithMessage("Invalid marital status value.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Governorate), () =>
        {
            RuleFor(x => x.Governorate)
                .MaximumLength(50).WithMessage("Governorate must not exceed 50 characters.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.District), () =>
        {
            RuleFor(x => x.District)
                .MaximumLength(50).WithMessage("District must not exceed 50 characters.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.AddressDetails), () =>
        {
            RuleFor(x => x.AddressDetails)
                .MaximumLength(200).WithMessage("Address details must not exceed 200 characters.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.PhotoUrl), () =>
        {
            RuleFor(x => x.PhotoUrl)
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters.");
        });
    }
}
