using Infrastructure.IServices;
using Models.Helpers;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(EmailSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }
    public void SendEmailAsync(string htmlMessage)
    {

        var emails = _emailSettings.ToEmails.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        var subject = _emailSettings.EmailSubject;
        string fromMail = _emailSettings.FromEmail;
        var fromMailAddress = new MailAddress(fromMail, "Schools Management System {FL}");
        string fromPassword = _emailSettings.Password;

        MailMessage message = new MailMessage();
        message.From = fromMailAddress;
        message.Subject = subject;
        foreach (var email in emails)
        {
            message.To.Add(new MailAddress(email));
        }
        message.Body = "<html><body> " + htmlMessage + " </body></html>";
        message.IsBodyHtml = true;

        SendMail(_emailSettings, fromMailAddress, fromPassword, message);
    }

    private static void SendMail(EmailSettings emailSettings, MailAddress fromMailAddress, string fromPassword, MailMessage message)
    {
        var smtpClient = new SmtpClient(emailSettings.SmtpServer)
        {
            Port = emailSettings.Port,
            Credentials = new NetworkCredential(fromMailAddress.Address, fromPassword),
            EnableSsl = emailSettings.EnableSsl,
            UseDefaultCredentials = false,
        };
        smtpClient.Send(message);

    }
}

