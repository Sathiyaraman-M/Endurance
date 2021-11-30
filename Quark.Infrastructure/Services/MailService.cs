using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quark.Core.Configurations;
using Quark.Core.Requests.Mail;
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
            Email.DefaultSender = new SmtpSender(() =>  new SmtpClient(origin)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = _config.Port
            });
            var email = await Email.From(_config.From).To(request.To).Subject(request.Subject).Body(request.Body).SendAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message, e);
        }
    }
}