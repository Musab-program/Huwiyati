namespace Huwiyati.Application.Requests.Validators;

using FluentValidation;
using Huwiyati.Application.Requests.Commands;
using Huwiyati.Domain.Enums;

/// <summary>
/// Validator for validating input parameters of <see cref="ChangeServiceRequestStatusCommand"/>.
/// </summary>
public class ChangeServiceRequestStatusCommandValidator : AbstractValidator<ChangeServiceRequestStatusCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeServiceRequestStatusCommandValidator"/> class.
    /// </summary>
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
