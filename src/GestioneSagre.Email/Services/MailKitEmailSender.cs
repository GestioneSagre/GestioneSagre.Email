using GestioneSagre.Email.Services.Interfaces;
using GestioneSagre.Models.InputModels.InvioEmail;
using GestioneSagre.Tools.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace GestioneSagre.Email.Services;

public class MailKitEmailSender : IEmailSenderService
{
    private readonly IOptionsMonitor<SmtpOptions> smtpOptionsMonitor;
    private readonly ILogger<MailKitEmailSender> logger;

    public MailKitEmailSender(IOptionsMonitor<SmtpOptions> smtpOptionsMonitor, ILogger<MailKitEmailSender> logger)
    {
        this.logger = logger;
        this.smtpOptionsMonitor = smtpOptionsMonitor;
    }

    public async Task SendEmailSupportoAsync(InputMailSupportoSender model, InputMailOptionSender optionSender)
    {
        try
        {
            var options = optionSender;

            using SmtpClient client = new();

            await client.ConnectAsync(options.Host, options.Port, options.Security);

            if (!string.IsNullOrEmpty(options.Username))
            {
                await client.AuthenticateAsync(options.Username, options.Password);
            }

            MimeMessage message = new();

            message.From.Add(MailboxAddress.Parse($"{model.MittenteNominativo} <{model.MittenteEmail}>"));
            message.To.Add(MailboxAddress.Parse($"{options.DestinatarioNominativo} <{options.DestinatarioEmail}>"));
            message.Subject = options.Oggetto;

            var builder = new BodyBuilder();

            builder.HtmlBody = model.Messaggio;
            message.Body = builder.ToMessageBody();

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}