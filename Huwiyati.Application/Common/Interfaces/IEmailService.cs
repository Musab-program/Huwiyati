namespace Huwiyati.Application.Common.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
    Task<bool> SendOtpEmailAsync(string toEmail, string subject, string otpCode, string purposeTitle, CancellationToken cancellationToken = default);
}

