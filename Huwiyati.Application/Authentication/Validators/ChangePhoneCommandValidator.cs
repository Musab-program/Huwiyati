namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for the <see cref="ChangePhoneCommand"/> command.
/// </summary>
public class ChangePhoneCommandValidator : AbstractValidator<ChangePhoneCommand>
{
    /// <summary>
    /// Initializes validation rules for phone number update requests.
    /// </summary>
    public ChangePhoneCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");

        RuleFor(x => x.NewPhoneNumber)
            .NotEmpty().WithMessage("New phone number is required.")
            .Matches(@"^[0-9+\-\s]{7,15}$").WithMessage("A valid phone number format is required.");
    }
}
