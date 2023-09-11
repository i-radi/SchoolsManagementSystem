namespace Infrastructure.Services;

public interface IEmailService
{
    void SendEmailAsync(string htmlMessage);
}