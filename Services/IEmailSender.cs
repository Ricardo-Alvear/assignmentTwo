namespace COMP2139___assignment2.Services;

public interface IEmailSender
{
    public Task SendEmailAsync(string to, string subject, string html);
}