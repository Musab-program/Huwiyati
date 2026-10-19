namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for the <see cref="ChangeEmailCommand"/> command.
/// </summary>
public class ChangeEmailCommandValidator : AbstractValidator<ChangeEmailCommand>
{
    /// <summary>
    /// Initializes validation rules for email change requests.
    /// </summary>
    public ChangeEmailCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");

        RuleFor(x => x.NewEmail)
            .NotEmpty().WithMessage("New email address is required.")
            .EmailAddress().WithMessage("A valid email address format is required.");
    }
}
