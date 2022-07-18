using GestioneSagre.Models.InputModels.InvioEmail;

namespace GestioneSagre.Email.Services.Interfaces;

public interface IEmailSenderService
{
    Task SendEmailSupportoAsync(InputMailSupportoSender model, InputMailOptionSender optionSender);
}