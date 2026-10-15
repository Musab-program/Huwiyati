namespace Huwiyati.API.Controllers.Authentication;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Huwiyati.Domain.Constants;
using Huwiyati.Application.Authentication.Commands;
using Huwiyati.Application.Authentication.DTOs;
using Huwiyati.Application.Authentication.Queries;

/// <summary>
/// Handles user authentication, registration, OTP verification, password recovery, device trust management, and account lifecycle endpoints.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly RegisterHandler _registerHandler;
    private readonly VerifyOTPHandler _verifyOtpHandler;
    private readonly LoginHandler _loginHandler;
    private readonly ForgotPasswordHandler _forgotPasswordHandler;
    private readonly VerifyResetHandler _verifyResetHandler;
    private readonly ResetPasswordHandler _resetPasswordHandler;
    private readonly VerifyDeviceHandler _verifyDeviceHandler;
    private readonly RemoveDeviceHandler _removeDeviceHandler;
    private readonly DeactivateAccountHandler _deactivateAccountHandler;
    private readonly ReactivateAccountHandler _reactivateAccountHandler;
    private readonly RequestAccountOtpHandler _requestAccountOtpHandler;
    private readonly GetCitizenAccountStatusHandler _getCitizenAccountStatusHandler;
    private readonly ActivateCitizenAccountHandler _activateCitizenAccountHandler;
    private readonly GetAllUserAccountsHandler _getAllUserAccountsHandler;
    private readonly RefreshTokenHandler _refreshTokenHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountController"/> class.
    /// </summary>
    public AccountController(
        RegisterHandler registerHandler,
        VerifyOTPHandler verifyOtpHandler,
        LoginHandler loginHandler,
        ForgotPasswordHandler forgotPasswordHandler,
        VerifyResetHandler verifyResetHandler,
        ResetPasswordHandler resetPasswordHandler,
        VerifyDeviceHandler verifyDeviceHandler,
        RemoveDeviceHandler removeDeviceHandler,
        DeactivateAccountHandler deactivateAccountHandler,
        ReactivateAccountHandler reactivateAccountHandler,
        RequestAccountOtpHandler requestAccountOtpHandler,
        GetCitizenAccountStatusHandler getCitizenAccountStatusHandler,
        ActivateCitizenAccountHandler activateCitizenAccountHandler,
        GetAllUserAccountsHandler getAllUserAccountsHandler,
        RefreshTokenHandler refreshTokenHandler)
    {
        _registerHandler = registerHandler;
        _verifyOtpHandler = verifyOtpHandler;
        _loginHandler = loginHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _verifyResetHandler = verifyResetHandler;
        _resetPasswordHandler = resetPasswordHandler;
        _verifyDeviceHandler = verifyDeviceHandler;
        _removeDeviceHandler = removeDeviceHandler;
        _deactivateAccountHandler = deactivateAccountHandler;
        _reactivateAccountHandler = reactivateAccountHandler;
        _requestAccountOtpHandler = requestAccountOtpHandler;
        _getCitizenAccountStatusHandler = getCitizenAccountStatusHandler;
        _activateCitizenAccountHandler = activateCitizenAccountHandler;
        _getAllUserAccountsHandler = getAllUserAccountsHandler;
        _refreshTokenHandler = refreshTokenHandler;
    }

    /// <summary>
    /// Registers a new citizen user account and dispatches an initial verification code.
    /// </summary>
    /// <param name="command">Registration payload containing personal details and credentials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating registration success or validation failure.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _registerHandler.CreateAccountAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Verifies the account activation OTP and enables the user account.
    /// </summary>
    /// <param name="command">OTP verification command containing national number and code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating verification outcome and token generation.</returns>
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPCommand command, CancellationToken cancellationToken)
    {
        var result = await _verifyOtpHandler.VerifyOtpAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Authenticates a user with national number and password, generating JWT and refresh tokens.
    /// </summary>
    /// <param name="command">Login credentials and optional device identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing authentication tokens and user profile.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _loginHandler.LoginHandlerAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Verifies a new or unrecognized device via OTP code to add it to trusted devices.
    /// </summary>
    /// <param name="command">Device verification details and OTP.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating whether the device was successfully verified.</returns>
    [HttpPost("verify-device")]
    public async Task<IActionResult> VerifyDevice([FromBody] VerifyDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await _verifyDeviceHandler.VerifyDeviceAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Requests a password reset verification code to be sent to user's registered phone or email.
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

    /// <summary>
    /// Removes a trusted device from the user account.
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

    /// <summary>
    /// Requests a new account OTP verification code.
    /// </summary>
    /// <param name="command">OTP request payload.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating OTP generation and dispatch.</returns>
    [HttpPost("request-otp")]
    public async Task<IActionResult> RequestOtp([FromBody] RequestAccountOtpCommand command, CancellationToken cancellationToken)
    {
        var result = await _requestAccountOtpHandler.RequestOtpAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Deactivates an active user account.
    /// </summary>
    /// <param name="command">Deactivation command with reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response indicating deactivation outcome.</returns>
    [HttpPost("deactivate")]
    public async Task<IActionResult> DeactivateAccount([FromBody] DeactivateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _deactivateAccountHandler.DeactivateAccountAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Reactivates a previously deactivated user account after verification.
    /// </summary>
    /// <param name="command">Reactivation credentials and OTP.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming account reactivation.</returns>
    [HttpPost("reactivate")]
    public async Task<IActionResult> ReactivateAccount([FromBody] ReactivateAccountCommand command, CancellationToken cancellationToken)
    {
        var result = await _reactivateAccountHandler.ReactivateAccountAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves the registration and account activation status of a citizen by national number.
    /// </summary>
    /// <param name="nationalNumber">The citizen national number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with citizen account status details.</returns>
    [HttpGet("citizen-status/{nationalNumber}")]
    [Authorize(Roles = "Employee, Admin")]
    public async Task<IActionResult> GetCitizenAccountStatus(string nationalNumber, CancellationToken cancellationToken)
    {
        var result = await _getCitizenAccountStatusHandler.GetCitizenAccountStatusAsync(nationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Activates a citizen account manually by an authorized civil registry employee or administrator.
    /// </summary>
    /// <param name="request">Request containing citizen national number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response confirming account activation.</returns>
    [HttpPost("activate-citizen")]
    [Authorize(Roles = "Employee, Admin")]
    public async Task<IActionResult> ActivateCitizenAccount([FromBody] ActivateCitizenAccountRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _activateCitizenAccountHandler.ActivateAccountAsync(request.NationalNumber, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Retrieves all user accounts across the platform. Accessible only by SuperAdmin.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response containing list of all registered user accounts.</returns>
    [HttpGet("all-users")]
    [Authorize(Roles = AppRoles.SuperAdmin)]
    public async Task<IActionResult> GetAllUserAccounts(CancellationToken cancellationToken)
    {
        var result = await _getAllUserAccountsHandler.GetAllUserAccountsAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Refreshes expired JWT access tokens using a valid refresh token.
    /// </summary>
    /// <param name="command">Payload containing access token and refresh token.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An API response with renewed JWT tokens.</returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var result = await _refreshTokenHandler.HandleAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
