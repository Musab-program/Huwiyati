using FluentValidation;
using Huwiyati.Application.Authentication.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Authentication.Validators
{
    public class VerifyOtpCommandValidator : AbstractValidator<VerifyOTPCommand>
    {
        public VerifyOtpCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x =>x.Code)
                .NotEmpty().WithMessage("Verification code is required.")
                .Length(6).WithMessage("Verification code must be exactly 6 digits.")
                .Matches(@"^\d{6}$").WithMessage("Verification code must contain only numbers.");

        }
    }
}
