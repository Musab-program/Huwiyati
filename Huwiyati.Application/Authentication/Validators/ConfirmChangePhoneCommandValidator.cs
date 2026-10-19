namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for the <see cref="ConfirmChangePhoneCommand"/> command.
/// </summary>
public class ConfirmChangePhoneCommandValidator : AbstractValidator<ConfirmChangePhoneCommand>
{
    /// <summary>
    /// Initializes validation rules for confirming phone number change.
    /// </summary>
    public ConfirmChangePhoneCommandValidator()
    {
        RuleFor(x => x.NewPhoneNumber)
            .NotEmpty().WithMessage("New phone number is required.")
            .Matches(@"^[0-9+\-\s]{7,15}$").WithMessage("A valid phone number format is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Verification code must be exactly 6 digits.");
    }
}
