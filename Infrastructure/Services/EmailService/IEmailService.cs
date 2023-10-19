namespace Infrastructure.Services;

public interface IEmailService
{
    Task SendEmailAsync(string htmlMessage);
}