using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace lib.api.Services;

public class EmailService(
    IOptions<EmailSettings> settings) : IEmailService
{
    private readonly EmailSettings _settings = settings.Value;
    
    public async Task SendEmail(string to, string body)
    {
        var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_settings.UserName, _settings.Password)
        };
        
        var message = new MailMessage
        {
            From = new MailAddress(_settings.FromEmail, _settings.FromName),
            Subject = "Library notifications: Return book tomorrow",
            Body = body,
            IsBodyHtml = false
        };
        
        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}

public class EmailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public bool EnableSsl { get; set; }
}