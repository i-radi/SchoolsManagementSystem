namespace Models.Helpers;

public class EmailSettings
{
    public bool SendEmails { get; set; }
    public string FromEmail { get; set; } = string.Empty;
    public string ToEmails { get; set; } = string.Empty;
    public string SmtpServer { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string EmailSubject { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
}