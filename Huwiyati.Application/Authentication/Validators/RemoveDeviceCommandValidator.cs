namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

public class RemoveDeviceCommandValidator : AbstractValidator<RemoveDeviceCommand>
{
    public RemoveDeviceCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.DeviceIdentifier)
            .NotEmpty().WithMessage("Device Identifier is required.");
    }
}