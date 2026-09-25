namespace Huwiyati.Infrastructure.Services;

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Huwiyati.Application.Common.Interfaces;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        var smtpServer = emailSettings["SmtpServer"] ?? "smtp.gmail.com";
        var port = int.Parse(emailSettings["Port"] ?? "587");
        var senderName = emailSettings["SenderName"] ?? "Huwiyati System";
        var senderEmail = emailSettings["SenderEmail"]!;
        var password = (emailSettings["Password"] ?? string.Empty).Replace(" ", "");

        if (string.IsNullOrWhiteSpace(password))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[EMAIL SERVICE - DEV MODE] Email to: {toEmail} | Subject: {subject}");
            Console.WriteLine($"[EMAIL SERVICE - DEV MODE] Body preview: {subject}");
            Console.ResetColor();
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(senderName, senderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(senderEmail, password, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[EMAIL SERVICE WARNING] Failed to send email via SMTP to {toEmail}: {ex.Message}");
            Console.ResetColor();
        }
    }

    public async Task SendOtpEmailAsync(string toEmail, string subject, string otpCode, string purposeTitle, CancellationToken cancellationToken = default)
    {
        var htmlBody = $@"
            <div style='font-family: Arial, sans-serif; text-align: right; direction: rtl; padding: 25px; border: 1px solid #e0e0e0; border-radius: 10px; max-width: 500px; margin: 0 auto; background-color: #ffffff;'>
                <h2 style='color: #004d40; margin-bottom: 5px; text-align: center;'>نظام هويتي الرقمية</h2>
                <hr style='border: none; border-top: 1px solid #eeeeee; margin: 15px 0;' />
                <p style='font-size: 16px; color: #333333;'>مرحباً بك،</p>
                <p style='font-size: 15px; color: #555555;'>رمز التحقق لـ <strong>{purposeTitle}</strong> هو:</p>
                <div style='text-align: center; margin: 25px 0;'>
                    <span style='font-size: 36px; font-weight: bold; color: #d32f2f; letter-spacing: 10px; background-color: #ffebee; padding: 12px 24px; border-radius: 8px; display: inline-block; font-family: monospace;'>{otpCode}</span>
                </div>
                <p style='font-size: 14px; color: #757575; text-align: center;'>هذا الرمز صالح لمدة 5 دقائق فقط. يُرجى عدم مشاركة هذا الرمز مع أي شخص آخر للحفاظ على أمان حسابك.</p>
                <div style='margin-top: 25px; padding-top: 15px; border-top: 1px solid #f0f0f0; text-align: center; font-size: 12px; color: #9e9e9e;'>
                    جميع الحقوق محفوظة &copy; {DateTime.UtcNow.Year} نظام هويتي الرقمية
                </div>
            </div>";

        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine($"\n=======================================================");
        Console.WriteLine($"[DEV OTP NOTIFICATION] To: {toEmail}");
        Console.WriteLine($"[DEV OTP CODE]: {otpCode}");
        Console.WriteLine($"=======================================================\n");
        Console.ResetColor();

        await SendEmailAsync(toEmail, subject, htmlBody, cancellationToken);
    }
}
