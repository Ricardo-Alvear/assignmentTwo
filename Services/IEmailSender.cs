namespace comp2147.Services;

public interface IEmailSender
{
    public Task SendEmailAsync(string to, string subject, string html);
}