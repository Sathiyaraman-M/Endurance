using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quark.Core.Configurations;
using Quark.Core.Requests.Mail;
using System.Net;
using System.Net.Mail;

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

    public async Task SendAsync(MailRequest request, string origin)
    {
        try
        {
            using var smtpClient = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = _config.EnableSSL,
                Host = _config.Host,
                Port = _config.Port,
                Credentials = new NetworkCredential(_config.UserName, _config.Password)
            };
            Email.DefaultSender = new SmtpSender(smtpClient);
            await Email.From(_config.UserName, _config.DisplayName).To(request.To).Subject(request.Subject).Body(request.Body).SendAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}