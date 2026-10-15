namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing required fields, length, and numerical digit format on <see cref="VerifyOTPCommand"/>.
/// </summary>
public class VerifyOtpCommandValidator : AbstractValidator<VerifyOTPCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="VerifyOtpCommandValidator"/> class and configures validation rules.
    /// </summary>
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Verification code must be exactly 6 digits.")
            .Matches(@"^\d{6}$").WithMessage("Verification code must contain only numbers.");
    }
}
