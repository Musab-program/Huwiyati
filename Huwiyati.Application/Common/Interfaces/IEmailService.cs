namespace Huwiyati.Application.Common.Interfaces;

/// <summary>
/// Defines email dispatching operations for system notifications and security verification codes.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends a general transactional email message to a specified recipient.
    /// </summary>
    /// <param name="toEmail">The destination email address.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="body">The plain text or HTML body content.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the email was sent successfully; otherwise, false.</returns>
    Task<bool> SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an OTP (one-time password) verification email with standard security styling.
    /// </summary>
    /// <param name="toEmail">The destination email address.</param>
    /// <param name="subject">The subject line of the email.</param>
    /// <param name="otpCode">The one-time passcode string.</param>
    /// <param name="purposeTitle">The operation or purpose for which the code was requested.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the OTP email was sent successfully; otherwise, false.</returns>
    Task<bool> SendOtpEmailAsync(string toEmail, string subject, string otpCode, string purposeTitle, CancellationToken cancellationToken = default);
}

