using Huwiyati.Application.Authentication.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Huwiyati.API.Controllers.Authentication;

using Microsoft.AspNetCore.Mvc;
using Huwiyati.Application.Authentication.Commands;

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

    public AccountController(
        RegisterHandler registerHandler,
        VerifyOTPHandler verifyOtpHandler,
        LoginHandler loginHandler,
        ForgotPasswordHandler forgotPasswordHandler,
        VerifyResetHandler verifyResetHandler,
        ResetPasswordHandler resetPasswordHandler,
        VerifyDeviceHandler verifyDeviceHandler)
    {
        _registerHandler = registerHandler;
        _verifyOtpHandler = verifyOtpHandler;
        _loginHandler = loginHandler;
        _forgotPasswordHandler = forgotPasswordHandler;
        _verifyResetHandler = verifyResetHandler;
        _resetPasswordHandler = resetPasswordHandler;
        _verifyDeviceHandler = verifyDeviceHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _registerHandler.CreateAccountAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOTP([FromBody] VerifyOTPCommand command, CancellationToken cancellationToken)
    {
        var result = await _verifyOtpHandler.VerifyOtpAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _loginHandler.LoginHandlerAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("verify-device")]
    public async Task<IActionResult> VerifyDevice([FromBody] VerifyDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await _verifyDeviceHandler.VerifyDeviceAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _forgotPasswordHandler.RequestPasswordResetAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("verify-reset-code")]
    public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCommand command, CancellationToken cancellationToken)
    {
        var result = await _verifyResetHandler.VerifyResetCodeAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await _resetPasswordHandler.ResetPasswordAsync(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
