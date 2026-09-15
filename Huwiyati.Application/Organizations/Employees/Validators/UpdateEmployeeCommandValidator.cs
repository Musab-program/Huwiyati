namespace Huwiyati.Application.Organizations.Employees.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Employees.Commands;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.NewBranchId)
            .NotEmpty().WithMessage("New Branch ID is required.");
    }
}
