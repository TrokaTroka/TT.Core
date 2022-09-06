using TT.Core.Domain.Constants;
using TT.Core.Domain.Entities;
using System.Reflection;
using System.Net.Mail;
using System.Net;
using TT.Core.Application.Interfaces.Helpers;
using TT.Core.Domain.Interfaces.Repositories;

namespace TT.Core.Application.Helpers;

public class MailHelper : IEmailHelper
{
    private readonly ILogErrorRepository _log;

    private readonly string ServiceName = "MailHelper";

    public MailHelper(ILogErrorRepository log)
    {
        _log = log;
    }

    public async Task<Attempt<Failure, bool>> Send(NotificationRequest notification)
    {
        var failure = new Failure();

        var mail = new MailMessage
        {
            From = new MailAddress(AppSettings.Instance.MailEmail, "Troka Troka"),
            Subject = notification.Title
        };

        mail.To.Add(notification.To);

        var body = LoadTemplate(notification.Template.Type.ToDescriptionString());

        if (!body.Succeeded)
        {
            failure.SetMessage("Erro ao enviar email");
            return failure;
        }

        if (notification.Template.Tags?.Count > 0)
            foreach (var tag in notification.Template.Tags)
                body.Success = body.Success.Replace($"[{tag.Key}]", tag.Value);

        mail.Body = body.Success;
        mail.IsBodyHtml = true;

        var client = new SmtpClient("smtp.office365.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(AppSettings.Instance.MailEmail, AppSettings.Instance.MailPassword),
            EnableSsl = true
        };

        try
        {
            await client.SendMailAsync(mail);

            return true;
        }
        catch (SmtpException ex)
        {
            failure.SetMessage(ex.Message);
            await _log.Create(new LogError(ServiceName, nameof(Send), ex.Message));
            return failure;
        }
    }

    private Attempt<Failure, string> LoadTemplate(string name = "Default.html")
    {
        var failure = new Failure();

        try
        {
            var template = string.Empty;

            var assembly = Assembly.GetCallingAssembly();

            var resourceName = $"TT.Core.Application.Template.{name}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using var reader = new StreamReader(stream);
                template = reader.ReadToEnd();
            }

            return template;
        }
        catch (SmtpException ex)
        {
            failure.SetMessage(ex.Message);
            _log.Create(new LogError(ServiceName, nameof(LoadTemplate), ex.Message));

            return failure;
        }
    }
}
