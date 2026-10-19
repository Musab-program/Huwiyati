namespace Huwiyati.API.Controllers.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Handles authenticated citizen profile updates, contact credentials (email, phone), device management, and account lifecycle.
/// Previously part of AccountController.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly ChangeEmailHandler _changeEmailHandler;
    private readonly ConfirmChangeEmailHandler _confirmChangeEmailHandler;
    private readonly ChangePhoneHandler _changePhoneHandler;
    private readonly ConfirmChangePhoneHandler _confirmChangePhoneHandler;
    private readonly RemoveDeviceHandler _removeDeviceHandler;
    private readonly DeactivateAccountHandler _deactivateAccountHandler;
    private readonly ReactivateAccountHandler _reactivateAccountHandler;
    private readonly RequestAccountOtpHandler _requestAccountOtpHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileController"/> class.
    /// </summary>
    public ProfileController(
        ChangeEmailHandler changeEmailHandler,
        ConfirmChangeEmailHandler confirmChangeEmailHandler,
        ChangePhoneHandler changePhoneHandler,
        ConfirmChangePhoneHandler confirmChangePhoneHandler,
        RemoveDeviceHandler removeDeviceHandler,
        DeactivateAccountHandler deactivateAccountHandler,
        ReactivateAccountHandler reactivateAccountHandler,
        RequestAccountOtpHandler requestAccountOtpHandler)
    {
        _changeEmailHandler = changeEmailHandler;
        _confirmChangeEmailHandler = confirmChangeEmailHandler;
        _changePhoneHandler = changePhoneHandler;
        _confirmChangePhoneHandler = confirmChangePhoneHandler;
        _removeDeviceHandler = removeDeviceHandler;
        _deactivateAccountHandler = deactivateAccountHandler;
        _reactivateAccountHandler = reactivateAccountHandler;
        _requestAccountOtpHandler = requestAccountOtpHandler;
    }

    /// <summary>
    /// Initiates email change for the authenticated citizen by validating current password and sending OTP to the new email.
    /// Route: POST api/v1/profile/change-email/request
    /// </summary>
    /// <param name="command">Payload containing current password and new email address.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating OTP dispatch status.</returns>
    [HttpPost("change-email/request")]
    [Authorize]
    public async Task<IActionResult> RequestChangeEmail([FromBody] ChangeEmailCommand command, CancellationToken cancellationToken)
    {
        var result = await _changeEmailHandler.HandleAsync(User, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Confirms email change for the authenticated user using the new email address and received OTP code.
    /// Route: POST api/v1/profile/change-email/confirm
    /// </summary>
    /// <param name="command">Payload containing the new email and 6-digit OTP code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming email update.</returns>
    [HttpPost("change-email/confirm")]
    [Authorize]
    public async Task<IActionResult> ConfirmChangeEmail([FromBody] ConfirmChangeEmailCommand command, CancellationToken cancellationToken)
    {
        var result = await _confirmChangeEmailHandler.HandleAsync(User, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Initiates phone number change for the authenticated user by validating current password and sending OTP to the registered email.
    /// Route: POST api/v1/profile/change-phone/request
    /// </summary>
    /// <param name="command">Payload containing current password and new phone number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating OTP dispatch status.</returns>
    [HttpPost("change-phone/request")]
    [Authorize]
    public async Task<IActionResult> RequestChangePhoneNumber([FromBody] ChangePhoneCommand command, CancellationToken cancellationToken)
    {
        var result = await _changePhoneHandler.HandleAsync(User, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Confirms phone number change for the authenticated user using the new phone number and received OTP code.
    /// Route: POST api/v1/profile/change-phone/confirm
    /// </summary>
    /// <param name="command">Payload containing the new phone number and 6-digit OTP code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming phone number update.</returns>
    [HttpPost("change-phone/confirm")]
    [Authorize]
    public async Task<IActionResult> ConfirmChangePhoneNumber([FromBody] ConfirmChangePhoneCommand command, CancellationToken cancellationToken)
    {
        var result = await _confirmChangePhoneHandler.HandleAsync(User, command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Requests a new account OTP verification code (used for sensitive actions like account deactivation or reactivation).
    /// Route: POST api/v1/profile/request-otp
    /// </summary>
    /// <param name="command">OTP request payload containing national number and password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating OTP generation and dispatch.</returns>
    [HttpPost("request-otp")]
    public async Task<IActionResult> RequestOtp([FromBody] RequestAccountOtpCommand command, CancellationToken cancellationToken)
    {
        var result = await _requestAccountOtpHandler.RequestOtpAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deactivates an active user account voluntarily after OTP verification.
    /// Route: POST api/v1/profile/deactivate-account
    /// </summary>
    /// <param name="command">Deactivation command with national number, password, reason, and OTP.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating deactivation outcome.</returns>
    [HttpPost("deactivate-account")]
    public async Task<IActionResult> DeactivateAccount([FromBody] DeactivateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _deactivateAccountHandler.DeactivateAccountAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Reactivates a previously deactivated user account after OTP verification.
    /// Route: POST api/v1/profile/reactivate-account
    /// </summary>
    /// <param name="command">Reactivation credentials and OTP.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming account reactivation.</returns>
    [HttpPost("reactivate-account")]
    public async Task<IActionResult> ReactivateAccount([FromBody] ReactivateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _reactivateAccountHandler.ReactivateAccountAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Removes a trusted device from the authenticated user's account.
    /// Route: POST api/v1/profile/remove-device
    /// </summary>
    /// <param name="command">Device removal details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming device revocation.</returns>
    [HttpPost("remove-device")]
    public async Task<IActionResult> RemoveDevice([FromBody] RemoveDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await _removeDeviceHandler.RemoveDeviceAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
