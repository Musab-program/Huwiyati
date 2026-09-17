namespace Huwiyati.Application.Family.Validators;

using FluentValidation;
using Huwiyati.Application.Family.Commands;

public class UpdateFamilyMemberStatusCommandValidator : AbstractValidator<UpdateFamilyMemberStatusCommand>
{
    public UpdateFamilyMemberStatusCommandValidator()
    {
        RuleFor(x => x.FamilyMemberId)
            .NotEmpty().WithMessage("Family member identifier is required.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid family member status specified.");

        When(x => x.LeftAt.HasValue, () =>
        {
            RuleFor(x => x.LeftAt!.Value)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Departure date (LeftAt) cannot be in the future.");
        });
    }
}