namespace Huwiyati.Application.Requests.Validators;

using FluentValidation;
using Huwiyati.Application.Requests.Commands;

/// <summary>
/// Validator for validating input parameters of <see cref="CreateServiceRequestCommand"/>.
/// </summary>
public class CreateServiceRequestCommandValidator : AbstractValidator<CreateServiceRequestCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateServiceRequestCommandValidator"/> class.
    /// </summary>
    public CreateServiceRequestCommandValidator()
    {
        RuleFor(v => v.ServiceTypeId)
            .NotEmpty().WithMessage("Service Type ID is required.");

        RuleFor(v => v.BranchId)
            .NotEmpty().WithMessage("Branch ID is required.");


    }
}
