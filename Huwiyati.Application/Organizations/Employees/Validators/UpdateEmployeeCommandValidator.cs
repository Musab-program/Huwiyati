namespace Huwiyati.Application.Organizations.Employees.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Employees.Commands;

/// <summary>
/// Validator for validating <see cref="UpdateEmployeeCommand"/> input rules.
/// </summary>
public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEmployeeCommandValidator"/> class.
    /// </summary>
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.NewBranchId)
            .NotEmpty().WithMessage("New Branch ID is required.");
    }
}
