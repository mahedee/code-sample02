using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ECommerceApi.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendWelcomeEmailAsync(string toEmail, string name)
    {
        var subject = "Welcome to ECommerce Store!";
        var body = $"""
            <h2>Welcome, {name}!</h2>
            <p>Thank you for registering at our store.</p>
            <p>Start browsing our products and place your first order today!</p>
            """;

        await SendEmailAsync(toEmail, name, subject, body);
    }

    public async Task SendOrderConfirmationAsync(string toEmail, string name, int orderId, decimal total)
    {
        var subject = $"Order #{orderId} Confirmed";
        var body = $"""
            <h2>Order Confirmed, {name}!</h2>
            <p>Your order <strong>#{orderId}</strong> has been placed successfully.</p>
            <p>Total Amount: <strong>${total:F2}</strong></p>
            <p>We will notify you when your order ships. Thank you for shopping with us!</p>
            """;

        await SendEmailAsync(toEmail, name, subject, body);
    }

    private async Task SendEmailAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var host = _config["Smtp:Host"]
            ?? throw new InvalidOperationException("SMTP host not configured.");
        var port = int.Parse(_config["Smtp:Port"] ?? "587");
        var username = _config["Smtp:Username"]
            ?? throw new InvalidOperationException("SMTP username not configured.");
        var password = _config["Smtp:Password"]
            ?? throw new InvalidOperationException("SMTP password not configured.");
        var fromEmail = _config["Smtp:FromEmail"] ?? username;
        var fromName = _config["Smtp:FromName"] ?? "ECommerce Store";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(username, password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        _logger.LogInformation("Email sent to {Email} — Subject: {Subject}", toEmail, subject);
    }
}
