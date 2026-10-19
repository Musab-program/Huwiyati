namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for the <see cref="ConfirmChangeEmailCommand"/> command.
/// </summary>
public class ConfirmChangeEmailCommandValidator : AbstractValidator<ConfirmChangeEmailCommand>
{
    /// <summary>
    /// Initializes validation rules for confirming email change.
    /// </summary>
    public ConfirmChangeEmailCommandValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty().WithMessage("New email address is required.")
            .EmailAddress().WithMessage("A valid email address format is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Verification code is required.")
            .Length(6).WithMessage("Verification code must be exactly 6 digits.");
    }
}
