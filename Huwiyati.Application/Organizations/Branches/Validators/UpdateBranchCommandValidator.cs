namespace Huwiyati.Application.Organizations.Branches.Validators;

using FluentValidation;
using Huwiyati.Application.Organizations.Branches.Commands;

public class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
{
    public UpdateBranchCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Branch ID is required.");

        RuleFor(x => x.OrganizationId)
            .NotEmpty().WithMessage("Organization ID is required.");

        RuleFor(x => x.BranchName)
            .NotEmpty().WithMessage("Branch name is required.")
            .MaximumLength(150).WithMessage("Branch name must not exceed 150 characters.");

        RuleFor(x => x.Governorate)
            .NotEmpty().WithMessage("Governorate is required.")
            .MaximumLength(100).WithMessage("Governorate must not exceed 100 characters.");

        RuleFor(x => x.District)
            .NotEmpty().WithMessage("District is required.")
            .MaximumLength(100).WithMessage("District must not exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).WithMessage("Phone number must not exceed 20 characters.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));
    }
}