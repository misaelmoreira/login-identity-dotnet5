using System;
using Microsoft.AspNetCore.Identity;

namespace App.Models
{
    public static class Inicializador
    {
        private static void InicializarPerfis(RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.RoleExistsAsync("administrador").Result)
            {
                var perfil = new IdentityRole<int>();
                perfil.Name = "administrador";
                roleManager.CreateAsync(perfil).Wait();
            }
        }

        private static void InicializarUsuarios(UserManager<UsuarioModel> userManager)
        {
            if (userManager.FindByNameAsync("admin@email.com").Result == null)
            {
                var usuario = new UsuarioModel();
                usuario.UserName = "admin@email.com";
                usuario.Email = "admin@email.com";
                usuario.NomeCompleto = "Administrador do Sistema";
                usuario.DataNascimento = new DateTime(1980, 1, 1);
                usuario.PhoneNumber = "99999999999";
                usuario.CPF = "00000000000";
                var resultado = userManager.CreateAsync(usuario, "123Aa@").Result;
                if (resultado.Succeeded)
                {
                    userManager.AddToRoleAsync(usuario, "administrador").Wait();
                }
            }
        }

        public static void InicializarIdentity(
            UserManager<UsuarioModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            InicializarPerfis(roleManager);
            InicializarUsuarios(userManager);
        }
    }
}