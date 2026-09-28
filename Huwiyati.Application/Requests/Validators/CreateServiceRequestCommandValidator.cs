namespace Huwiyati.Application.Requests.Validators;

using FluentValidation;
using Huwiyati.Application.Requests.Commands;

// Validator defining FluentValidation rules for CreateServiceRequestCommand
public class CreateServiceRequestCommandValidator : AbstractValidator<CreateServiceRequestCommand>
{
    public CreateServiceRequestCommandValidator()
    {
        RuleFor(v => v.ServiceTypeId)
            .NotEmpty().WithMessage("Service Type ID is required.");

        RuleFor(v => v.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");


    }
}
