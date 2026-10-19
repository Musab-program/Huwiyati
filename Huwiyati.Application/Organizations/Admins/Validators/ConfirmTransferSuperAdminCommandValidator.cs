namespace Huwiyati.Application.Organizations.Admins.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Validator for verifying the validity of <see cref="ConfirmTransferSuperAdminCommand"/> payload.
/// </summary>
public class ConfirmTransferSuperAdminCommandValidator : AbstractValidator<ConfirmTransferSuperAdminCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmTransferSuperAdminCommandValidator"/> class.
    /// </summary>
    public ConfirmTransferSuperAdminCommandValidator()
    {
        RuleFor(x => x.TargetNationalNumber)
            .NotEmpty().WithMessage("Target national number is required.")
            .MaximumLength(20).WithMessage("National number must not exceed 20 characters.");

        RuleFor(x => x.OtpCode)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Verification code must be exactly 6 digits.");
    }
}
