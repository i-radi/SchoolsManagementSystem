using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Models.Helpers;
using System.Net.Mail;

namespace Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public EmailService(EmailSettings emailSettings, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _emailSettings = emailSettings;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task SendEmailAsync(string htmlMessage)
    {

        var emails = _emailSettings.ToEmails.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        var subject = _emailSettings.EmailSubject;
        string fromMail = _emailSettings.FromEmail;
        var fromMailAddress = new MailAddress(fromMail, "Schools Management System {FL}");

        MailMessage message = new MailMessage();
        message.From = fromMailAddress;
        message.Subject = subject;
        foreach (var email in emails)
        {
            message.To.Add(new MailAddress(email));
        }
        message.Body = "<html><body> " + htmlMessage + " </body></html>";
        message.IsBodyHtml = true;
        var clientId = _configuration["Google:ClientId"]!;
        var clientSecret = _configuration["Google:ClientSecret"]!;

        await SendMailAsync(_emailSettings, fromMailAddress, clientId, clientSecret, message);
    }

    private async Task SendMailAsync(EmailSettings emailSettings, MailAddress fromMailAddress, string clientId, string clientSecret, MailMessage message)
    {
        var credential = await GetOAuth2CredentialAsync(clientId, clientSecret);

        var service = new GmailService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "SMS Application"
        });

        var mimeMessage = MimeKit.MimeMessage.CreateFromMailMessage(message);
        var gmailMessage = new Message
        {
            Raw = Base64UrlEncode(mimeMessage.ToString())
        };

        await service.Users.Messages.Send(gmailMessage, "me").ExecuteAsync();
    }

    private async Task<UserCredential> GetOAuth2CredentialAsync(string clientId, string clientSecret)
    {
        String FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "google credentials");

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                                new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret },
                                new[] { GmailService.Scope.GmailSend },
                                "user",
                                CancellationToken.None,
                                new FileDataStore(FilePath, true));

        return credential;
    }

    private string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(inputBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }
}