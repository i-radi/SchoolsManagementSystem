namespace Infrastructure.Services;

public interface IEmailService
{
    void SendEmail(string htmlMessage);
}