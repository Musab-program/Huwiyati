namespace Huwiyati.Application.Requests.Validators;

using FluentValidation;
using Huwiyati.Application.Requests.Commands;

public class CancelServiceRequestCommandValidator : AbstractValidator<CancelServiceRequestCommand>
{
    public CancelServiceRequestCommandValidator()
    {
        RuleFor(v => v.ServiceRequestId)
            .NotEmpty().WithMessage("Service Request ID is required.");
    }
}
