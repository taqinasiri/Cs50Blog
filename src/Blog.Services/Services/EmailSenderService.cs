using Blog.ViewModels.Application;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Blog.Services.Services;
public class EmailSenderService : IEmailSenderService
{
    private readonly EmailConfigsModel _emailConfig;
    private readonly IWebHostEnvironment _env;

    public EmailSenderService(IOptionsSnapshot<EmailConfigsModel> emailConfig, IWebHostEnvironment env)
    {
        _emailConfig = emailConfig.Value;
        _env = env;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_emailConfig.SiteTitle, _emailConfig.SiteAddress));
        mimeMessage.To.Add(new MailboxAddress("", to));
        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart(TextFormat.Html) { Text = body };

        if (_env.IsDevelopment())
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "test-emails", $"Blog-{Guid.NewGuid():N}.eml");
            await using var stream = new FileStream(path, FileMode.Create);
            await mimeMessage.WriteToAsync(stream);
        }
        else
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, _emailConfig.UseSSL).ConfigureAwait(false);
            await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password).ConfigureAwait(false);
            await client.SendAsync(mimeMessage).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}
