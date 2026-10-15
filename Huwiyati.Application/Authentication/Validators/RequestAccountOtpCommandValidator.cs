namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing required fields on <see cref="RequestAccountOtpCommand"/>.
/// </summary>
public class RequestAccountOtpCommandValidator : AbstractValidator<RequestAccountOtpCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestAccountOtpCommandValidator"/> class and configures validation rules.
    /// </summary>
    public RequestAccountOtpCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
