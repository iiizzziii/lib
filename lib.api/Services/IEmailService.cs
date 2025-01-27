namespace lib.api.Services;

public interface IEmailService
{
    Task SendEmail(string to, string body);
}