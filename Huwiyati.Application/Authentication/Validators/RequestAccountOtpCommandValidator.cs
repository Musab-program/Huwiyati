namespace Huwiyati.Application.Authentication.Validators;

using FluentValidation;
using Huwiyati.Application.Authentication.Commands;

public class RequestAccountOtpCommandValidator : AbstractValidator<RequestAccountOtpCommand>
{
    public RequestAccountOtpCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National Number is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
