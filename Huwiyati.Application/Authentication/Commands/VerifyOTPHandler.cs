using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Authentication.Commands
{
    public class VerifyOTPHandler
    {
        private readonly IApplicationDbContext _context;
        public VerifyOTPHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<bool>> VerifyOtpAsync(VerifyOTPCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Search for the active verification code for the given user
            var verificationCode = await _context.VerificationCodes
                .Where(vc => vc.UserId == command.UserId && !vc.IsUsed)
                .OrderByDescending(vc => vc.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            // 2. Check if code exists
            if (verificationCode == null)
            {
                return ApiResponse<bool>.Failure(
                    "Invalid verification code or no active code found.",
                    statusCode: 400);
            }

            // 3. Check code expiration
            if (verificationCode.ExpirationTime < DateTime.UtcNow)
            {
                return ApiResponse<bool>.Failure(
                    "Verification code has expired. Please request a new one.",
                    statusCode: 400);
            }

            // 4. Validate code matching
            if (verificationCode.Code != command.Code)
            {
                return ApiResponse<bool>.Failure(
                    "Invalid verification code.",
                    statusCode: 400);
            }

            // 5. Mark code as used
            verificationCode.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);

            // 6. Return success (Account remains PendingActivation for physical verification)
            return ApiResponse<bool>.Success(
                true,
                message: "OTP verification succeeded. Account is awaiting physical verification.");
        }
    }
}
