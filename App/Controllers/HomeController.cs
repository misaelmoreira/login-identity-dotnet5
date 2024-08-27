using System.Text;
using System.Threading.Tasks;
using App.Extensions;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailService _emailService;

        public HomeController(IEmailService emailService)
        {
            this._emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EnviarEmailTeste()
        {
            var html = new StringBuilder();
            html.Append("<h1>Teste de Serviço de Envio de E-mail</h1>");
            html.Append("<p>Este é um teste do serviço de envio de e-mails usando ASP.NET Core.</p>");
            await _emailService.SendEmailAsync("contato@maroquio.com", "Teste de Serviço de E-mail", string.Empty, html.ToString());
            this.MostrarMensagem("Uma mensagem foi enviada para o e-mail contato@maroquio.com.");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Policy = "MaiorDeIdade")]
        public IActionResult ParaMaiores()
        {
            return View();
        }
    }
}