using Blog.Services.Contracts;
using Blog.ViewModels.Application;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Blog.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IOptionsSnapshot<EmailConfigsModel> _emailConfig;
    private readonly IWebHostEnvironment _env;

    public EmailSenderService(IOptionsSnapshot<EmailConfigsModel> emailConfig, IWebHostEnvironment env)
    {
        _emailConfig = emailConfig;
        _env = env;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_emailConfig.Value.SiteTitle, _emailConfig.Value.SiteAddress));
        mimeMessage.To.Add(new MailboxAddress("", to));
        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart(TextFormat.Html)
        {
            Text = body
        };
        if (_env.IsDevelopment())
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "emails", $"Blog-{Guid.NewGuid():N}.eml");
            await using var stream = new FileStream(path, FileMode.CreateNew);
            await mimeMessage.WriteToAsync(stream);
        }
        else
        {
            using var client = new SmtpClient();
            //client.LocalDomain = "";
            await client.ConnectAsync(_emailConfig.Value.Host, _emailConfig.Value.Port, _emailConfig.Value.UseSSL).ConfigureAwait(false);
            await client.AuthenticateAsync(_emailConfig.Value.UserName, _emailConfig.Value.Password).ConfigureAwait(false);
            await client.SendAsync(mimeMessage).ConfigureAwait(false);
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}

