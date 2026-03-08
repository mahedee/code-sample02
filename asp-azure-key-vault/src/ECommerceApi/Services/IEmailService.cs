namespace ECommerceApi.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string toEmail, string name);
    Task SendOrderConfirmationAsync(string toEmail, string name, int orderId, decimal total);
}
