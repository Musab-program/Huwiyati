namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Validator for enforcing business rules and required attributes on <see cref="ForgotPasswordCommand"/>.
/// </summary>
public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForgotPasswordValidator"/> class and configures validation rules.
    /// </summary>
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");
    }
}