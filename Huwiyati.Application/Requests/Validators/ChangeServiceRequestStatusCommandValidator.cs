namespace Huwiyati.Application.Requests.Validators;

using FluentValidation;
using Huwiyati.Application.Requests.Commands;
using Huwiyati.Domain.Enums;

public class ChangeServiceRequestStatusCommandValidator : AbstractValidator<ChangeServiceRequestStatusCommand>
{
    public ChangeServiceRequestStatusCommandValidator()
    {
        RuleFor(v => v.ServiceRequestId)
            .NotEmpty().WithMessage("Service Request ID is required.");

        RuleFor(v => v.NewStatus)
            .IsInEnum().WithMessage("Invalid request status specified.");

        RuleFor(v => v.RejectionReason)
            .NotEmpty()
            .When(v => v.NewStatus == RequestStatus.Rejected)
            .WithMessage("Rejection reason is strictly required when rejecting a request.");
    }
}
