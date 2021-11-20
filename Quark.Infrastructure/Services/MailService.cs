using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Quark.Core.Configurations;
using Quark.Core.Requests.Mail;

namespace Quark.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly MailConfiguration _config;
    private readonly ILogger<MailService> _logger;

    public MailService(IOptions<MailConfiguration> config, ILogger<MailService> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task SendAsync(MailRequest request)
    {
        try
        {
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(_config.DisplayName, request.From ?? _config.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            await smtp.ConnectAsync(_config.Host, _config.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.UserName, _config.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}