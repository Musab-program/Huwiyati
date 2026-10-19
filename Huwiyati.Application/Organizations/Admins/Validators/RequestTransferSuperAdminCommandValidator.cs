namespace Huwiyati.Application.Organizations.Admins.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Validator for verifying the validity of <see cref="RequestTransferSuperAdminCommand"/> payload.
/// </summary>
public class RequestTransferSuperAdminCommandValidator : AbstractValidator<RequestTransferSuperAdminCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestTransferSuperAdminCommandValidator"/> class.
    /// </summary>
    public RequestTransferSuperAdminCommandValidator()
    {
        RuleFor(x => x.SuperAdminNationalNumber)
            .NotEmpty().WithMessage("SuperAdmin national number is required.")
            .MaximumLength(20).WithMessage("National number must not exceed 20 characters.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required.");
    }
}
