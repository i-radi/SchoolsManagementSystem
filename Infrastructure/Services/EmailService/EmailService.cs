using MailKit.Net.Smtp;
using MimeKit;
using Models.Helpers;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }
    public void SendEmail(string htmlMessage)
    {
        var emailMessage = CreateEmailMessage(htmlMessage);
        Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(string htmlMessage)
    {
        List<MailboxAddress> emails = _emailSettings.ToEmails
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new MailboxAddress(x, x)).ToList();

        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailSettings.FromEmail, _emailSettings.FromEmail));
        emailMessage.To.AddRange(emails);
        emailMessage.Subject = _emailSettings.EmailSubject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };
        return emailMessage;
    }

    private void Send(MimeMessage message)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_emailSettings.SmtpServer, _emailSettings.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailSettings.UserName, _emailSettings.Password);
                client.Send(message);
            }
            catch
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

    }
}