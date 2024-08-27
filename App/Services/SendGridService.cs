using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using App.Settings;

namespace App.Services
{
    public class SendGridService : IEmailService
    {
        private readonly SendGridSettings _emailSettings;

        public SendGridService(IOptions<SendGridSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string assunto, string mensagemTexto, string mensagemHtml)
        {
            var sgc = new SendGridClient(_emailSettings.ApiKey);
            var remetente = new EmailAddress(_emailSettings.EmailRemetente, _emailSettings.NomeRemetente);
            var destinatario = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(remetente, destinatario, assunto, mensagemTexto, mensagemHtml);
            await sgc.SendEmailAsync(msg);
        }
    }
}