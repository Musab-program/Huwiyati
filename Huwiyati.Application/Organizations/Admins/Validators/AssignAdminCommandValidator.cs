namespace Huwiyati.Application.Organizations.Admins.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Admins.Commands;

/// <summary>
/// Validator for verifying the validity of <see cref="AssignAdminCommand"/> data.
/// </summary>
public class AssignAdminCommandValidator : AbstractValidator<AssignAdminCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignAdminCommandValidator"/> class.
    /// </summary>
    public AssignAdminCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National number is required.")
            .MaximumLength(20).WithMessage("National number must not exceed 20 characters.");

        RuleFor(x => x.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");

       
    }
}
