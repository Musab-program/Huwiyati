namespace Huwiyati.Application.Organizations.Employees.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Employees.Commands;

/// <summary>
/// Validator for validating <see cref="AssignEmployeeCommand"/> input rules.
/// </summary>
public class AssignEmployeeCommandValidator : AbstractValidator<AssignEmployeeCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssignEmployeeCommandValidator"/> class.
    /// </summary>
    public AssignEmployeeCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National number is required.")
            .Length(10).WithMessage("National number must be 10 digits.");
    }
}
