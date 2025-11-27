using COMP2139___assignment2.Services;
using MailKit.Net.Smtp;
using MimeKit;

namespace comp2147.Services;

public class MailKitEmailSender : IEmailSender
{
    private readonly IConfiguration _config;
    public MailKitEmailSender(IConfiguration config) => _config = config;

    public async Task SendEmailAsync(string to, string subject, string html)
    {
        var msg = new MimeMessage();

        // Correct mapping to your JSON keys
        msg.From.Add(new MailboxAddress(
            "EventApp",                       // Hard-coded OR replace with a config key later
            _config["Smtp:From"]              // This exists in your JSON
        ));

        msg.To.Add(MailboxAddress.Parse(to));
        msg.Subject = subject;
        msg.Body = new BodyBuilder { HtmlBody = html }.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _config["Smtp:Host"],
            int.Parse(_config["Smtp:Port"]!),
            MailKit.Security.SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            _config["Smtp:User"],
            _config["Smtp:Pass"]
        );

        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }
}