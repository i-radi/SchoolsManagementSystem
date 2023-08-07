namespace Infrastructure.IServices;

public interface IEmailSender
{
    void SendEmailAsync(string htmlMessage);
}