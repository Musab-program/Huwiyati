namespace Huwiyati.API.Controllers.Authentication;

using Microsoft.AspNetCore.Mvc;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Handles password recovery, reset OTP verification, and password reset endpoints.
/// Previously part of AccountController.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PasswordController : ControllerBase
{
    private readonly ForgotPasswordHandler _forgotPasswordHandler;
    private readonly VerifyResetHandler _verifyResetHandler;
    private readonly ResetPasswordHandler _resetPasswordHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordController"/> class.
    /// </summary>
    public PasswordController(
        ForgotPasswordHandler forgotPasswordHandler,
        VerifyResetHandler verifyResetHandler,
        ResetPasswordHandler resetPasswordHandler)
    {
        _forgotPasswordHandler = forgotPasswordHandler;
        _verifyResetHandler = verifyResetHandler;
        _resetPasswordHandler = resetPasswordHandler;
    }

    /// <summary>
    /// Requests a password reset verification code to be sent to user's registered phone or email.
    /// Route: POST api/v1/password/forgot-password
    /// </summary>
    /// <param name="command">Identifier of the user requesting password reset.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating reset request dispatch.</returns>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _forgotPasswordHandler.RequestPasswordResetAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Verifies the password reset code before allowing the password change.
    /// Route: POST api/v1/password/verify-reset-code
    /// </summary>
    /// <param name="command">Verification code command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with reset validation token or status.</returns>
    [HttpPost("verify-reset-code")]
    public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCommand command, CancellationToken cancellationToken)
    {
        var result = await _verifyResetHandler.VerifyResetCodeAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Resets the user account password using the verified reset code.
    /// Route: POST api/v1/password/reset-password
    /// </summary>
    /// <param name="command">New password and verification token.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating password reset completion.</returns>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _resetPasswordHandler.ResetPasswordAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
