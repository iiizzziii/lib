using System.Net;
using System.Net.Mail;

namespace lib.api.Services;

public class EmailService : IEmailService
{
    public void SendEmail(string to, string subject, string body)
    {
        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 25)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("f4c592a94a9745", "********8b5b")
        };
        
        client.Send("library@library.com", to, subject, body);
    }
}