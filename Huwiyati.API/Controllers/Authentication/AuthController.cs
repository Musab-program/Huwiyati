namespace Huwiyati.API.Controllers.Authentication;

using Microsoft.AspNetCore.Mvc;
using Huwiyati.Application.Authentication.Commands;

/// <summary>
/// Handles user onboarding, registration, authentication, device verification, and token refresh endpoints.
/// Previously part of AccountController.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly RegisterHandler _registerHandler;
    private readonly VerifyOTPHandler _verifyOtpHandler;
    private readonly LoginHandler _loginHandler;
    private readonly VerifyDeviceHandler _verifyDeviceHandler;
    private readonly RefreshTokenHandler _refreshTokenHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    public AuthController(
        RegisterHandler registerHandler,
        VerifyOTPHandler verifyOtpHandler,
        LoginHandler loginHandler,
        VerifyDeviceHandler verifyDeviceHandler,
        RefreshTokenHandler refreshTokenHandler)
    {
        _registerHandler = registerHandler;
        _verifyOtpHandler = verifyOtpHandler;
        _loginHandler = loginHandler;
        _verifyDeviceHandler = verifyDeviceHandler;
        _refreshTokenHandler = refreshTokenHandler;
    }

    /// <summary>
    /// Registers a new citizen user account and dispatches an initial verification code.
    /// Route: POST api/v1/auth/register
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
    /// Route: POST api/v1/auth/verify-otp
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
    /// Route: POST api/v1/auth/login
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
    /// Route: POST api/v1/auth/verify-device
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
    /// Refreshes expired JWT access tokens using a valid refresh token.
    /// Route: POST api/v1/auth/refresh-token
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
