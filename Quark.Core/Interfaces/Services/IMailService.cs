using Quark.Core.Requests.Mail;

namespace Quark.Core.Interfaces.Services;

public interface IMailService
{
    Task SendAsync(MailRequest request);
}