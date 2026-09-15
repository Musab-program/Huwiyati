namespace Huwiyati.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
    Task SendOtpEmailAsync(string toEmail, string subject, string otpCode, string purposeTitle, CancellationToken cancellationToken = default);
}
