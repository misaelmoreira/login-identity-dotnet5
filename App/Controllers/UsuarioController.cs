using App.Extensions;
using App.Models;
using App.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UserManager<UsuarioModel> _userManager;
        private readonly SignInManager<UsuarioModel> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public UsuarioController(UserManager<UsuarioModel> userManager,
            SignInManager<UsuarioModel> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Cadastrar(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var usuarioBD = await _userManager.FindByIdAsync(id);
                if (usuarioBD == null)
                {
                    this.MostrarMensagem("Usuário não encontrado.", true);
                    return RedirectToAction("Index", "Home");
                }
                var usuarioVM = new CadastrarUsuarioViewModel
                {
                    Id = usuarioBD.Id,
                    NomeCompleto = usuarioBD.NomeCompleto,
                    DataNascimento = usuarioBD.DataNascimento,
                    CPF = usuarioBD.CPF,
                    Email = usuarioBD.Email,
                    Telefone = usuarioBD.PhoneNumber
                };
                return View(usuarioVM);
            }
            return View(new CadastrarUsuarioViewModel());
        }

        private bool EntidadeExiste(int id)
        {
            return (_userManager.Users.AsNoTracking().Any(u => u.Id == id));
        }

        private static void MapearCadastrarUsuarioViewModel(CadastrarUsuarioViewModel entidadeOrigem, UsuarioModel entidadeDestino)
        {
            entidadeDestino.NomeCompleto = entidadeOrigem.NomeCompleto;
            entidadeDestino.DataNascimento = entidadeOrigem.DataNascimento;
            entidadeDestino.CPF = entidadeOrigem.CPF;
            entidadeDestino.UserName = entidadeOrigem.Email;
            entidadeDestino.NormalizedUserName = entidadeOrigem.Email.ToUpper().Trim();
            entidadeDestino.Email = entidadeOrigem.Email;
            entidadeDestino.NormalizedEmail = entidadeOrigem.Email.ToUpper().Trim();
            entidadeDestino.PhoneNumber = entidadeOrigem.Telefone;
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(
        [FromForm] CadastrarUsuarioViewModel usuarioVM)
        {
            //se for alteração, não tem senha e confirmação de senha
            if (usuarioVM.Id > 0)
            {
                ModelState.Remove("Senha");
                ModelState.Remove("ConfSenha");
            }

            if (ModelState.IsValid)
            {
                if (EntidadeExiste(usuarioVM.Id))
                {
                    var usuarioBD = await _userManager.FindByIdAsync(usuarioVM.Id.ToString());
                    if ((usuarioVM.Email != usuarioBD.Email) &&
                        (_userManager.Users.Any(u => u.NormalizedEmail == usuarioVM.Email.ToUpper().Trim())))
                    {
                        ModelState.AddModelError("Email",
                            "Já existe um usuário cadastrado com este e-mail.");
                        return View(usuarioVM);
                    }
                    MapearCadastrarUsuarioViewModel(usuarioVM, usuarioBD);

                    var resultado = await _userManager.UpdateAsync(usuarioBD);
                    if (resultado.Succeeded)
                    {
                        var dataNascimentoClaim = new Claim(ClaimTypes.DateOfBirth,
                            usuarioBD.DataNascimento.Date.ToShortDateString());
                        await _userManager.AddClaimAsync(usuarioBD, dataNascimentoClaim);
                        this.MostrarMensagem("Usuário alterado com sucesso.");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        this.MostrarMensagem("Não foi possível alterar o usuário.", true);
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(usuarioVM);
                    }
                }
                else
                {
                    var usuarioBD = await _userManager.FindByEmailAsync(usuarioVM.Email);
                    if (usuarioBD != null)
                    {
                        ModelState.AddModelError("Email",
                            "Já existe um usuário cadastrado com este e-mail.");
                        return View(usuarioBD);
                    }

                    usuarioBD = new UsuarioModel();
                    MapearCadastrarUsuarioViewModel(usuarioVM, usuarioBD);

                    var resultado = await _userManager.CreateAsync(
                        usuarioBD, usuarioVM.Senha);
                    if (resultado.Succeeded)
                    {
                        var dataNascimentoClaim = new Claim(ClaimTypes.DateOfBirth,
                            usuarioBD.DataNascimento.Date.ToShortDateString());
                        await _userManager.AddClaimAsync(usuarioBD, dataNascimentoClaim);
                        this.MostrarMensagem("Usuário cadastrado com sucesso. Use suas credenciais para entrar no sistema.");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        this.MostrarMensagem("Erro ao cadastrar usuário.", true);
                        foreach (var error in resultado.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(usuarioVM);
                    }
                }
            }
            else
            {
                return View(usuarioVM);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            var login = new LoginViewModel();
            return View(login);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(login.Usuario, login.Senha, login.Lembrar, false);
                if (resultado.Succeeded)
                {
                    login.ReturnUrl = login.ReturnUrl ?? "~/";
                    return LocalRedirect(login.ReturnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tentativa de login inválida. Reveja seus dados de acesso e tente novamente.");
                    return View(login);
                }
            }
            else
            {
                return View(login);
            }
        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.AsNoTracking().ToListAsync();
            var admins = (await _userManager.GetUsersInRoleAsync("administrador"))
                .Select(u => u.UserName);
            ViewBag.Administradores = admins;
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Excluir(int? id)
        {
            if (!id.HasValue)
            {
                this.MostrarMensagem("Usuário não informado.", true);
                return RedirectToAction(nameof(Index));
            }

            if (!EntidadeExiste(id.Value))
            {
                this.MostrarMensagem("Usuário não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.FindByIdAsync(id.ToString());

            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirPost(int id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario != null)
            {
                var resultado = await _userManager.DeleteAsync(usuario);
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem("Usuário excluído com sucesso.");
                }
                else
                {
                    this.MostrarMensagem("Não foi possível excluir o usuário.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Usuário não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult AcessoRestrito([FromQuery] string returnUrl)
        {
            return View(model: returnUrl);
        }

        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> AddAdministrador(int id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario != null)
            {
                var resultado = await _userManager.AddToRoleAsync(usuario, "administrador");
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem(
                        $"Perfil administrador adicionado com sucesso para <b>{usuario.UserName}</b>.");
                }
                else
                {
                    this.MostrarMensagem(
                        $"Não foi possível adicionar perfil administrador para <b>{usuario.UserName}</b>.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Usuário não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "administrador")]
        public async Task<IActionResult> RemAdministrador(int id)
        {
            var usuario = await _userManager.FindByIdAsync(id.ToString());
            if (usuario != null)
            {
                var resultado = await _userManager.RemoveFromRoleAsync(usuario, "administrador");
                if (resultado.Succeeded)
                {
                    this.MostrarMensagem(
                        $"Perfil administrador removido com sucesso de <b>{usuario.UserName}</b>.");
                }
                else
                {
                    this.MostrarMensagem(
                        $"Não foi possível remover perfil administrador de <b>{usuario.UserName}</b>.", true);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                this.MostrarMensagem("Usuário não encontrado.", true);
                return RedirectToAction(nameof(Index));
            }
        }
    }
}