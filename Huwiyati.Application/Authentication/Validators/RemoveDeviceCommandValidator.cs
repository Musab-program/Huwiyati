namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing required fields on <see cref="RemoveDeviceCommand"/>.
/// </summary>
public class RemoveDeviceCommandValidator : AbstractValidator<RemoveDeviceCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveDeviceCommandValidator"/> class and configures validation rules.
    /// </summary>
    public RemoveDeviceCommandValidator()
    {
        RuleFor(x => x.DeviceIdentifier)
            .NotEmpty().WithMessage("Device Identifier is required.");
    }
}
