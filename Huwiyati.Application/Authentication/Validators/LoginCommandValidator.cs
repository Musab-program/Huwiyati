namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing business rules and required attributes on <see cref="LoginCommand"/>.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class and configures validation rules.
    /// </summary>
    public LoginCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(x => x.RequestedRole)
            .NotEmpty().WithMessage("Requested Role is required.");

        RuleFor(x => x.DeviceIdentifier)
            .NotEmpty().WithMessage("Device Identifier is required.");
    }
}
