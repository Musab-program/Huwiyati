namespace Huwiyati.Application.Documents.NationalIdCard.Validators;

using FluentValidation;
using Huwiyati.Application.Documents.NationalIdCard.Commands;

/// <summary>
/// Validator defining validation rules for <see cref="RenewNationalIdCardCommand"/>.
/// </summary>
public class RenewNationalIdCardCommandValidator : AbstractValidator<RenewNationalIdCardCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RenewNationalIdCardCommandValidator"/> class and configures validation rules.
    /// </summary>
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
