namespace Huwiyati.Application.Requests.Validators;

using FluentValidation;
using Huwiyati.Application.Requests.Commands;

/// <summary>
/// Validator for validating input parameters of <see cref="CancelServiceRequestCommand"/>.
/// </summary>
public class CancelServiceRequestCommandValidator : AbstractValidator<CancelServiceRequestCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CancelServiceRequestCommandValidator"/> class.
    /// </summary>
    public CancelServiceRequestCommandValidator()
    {
        RuleFor(v => v.ServiceRequestId)
            .NotEmpty().WithMessage("Service Request ID is required.");
    }
}
