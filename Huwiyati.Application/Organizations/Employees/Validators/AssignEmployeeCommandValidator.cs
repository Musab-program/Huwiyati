namespace Huwiyati.Application.Organizations.Employees.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Employees.Commands;

public class AssignEmployeeCommandValidator : AbstractValidator<AssignEmployeeCommand>
{
    public AssignEmployeeCommandValidator()
    {
        RuleFor(x => x.NationalNumber)
            .NotEmpty().WithMessage("National number is required.")
            .Length(10).WithMessage("National number must be 10 digits.");
    }
}
