using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace App.Policies
{
    public class MaiorDeIdadeRequirement : IAuthorizationRequirement
    {
        public int IdadeMinima { get; set; }

        public MaiorDeIdadeRequirement(int idadeMinima)
        {
            this.IdadeMinima = idadeMinima;
        }
    }

    public class MaiorDeIdadeHandler : AuthorizationHandler<MaiorDeIdadeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, MaiorDeIdadeRequirement requirement)
        {
            if (!(context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth)))
                return Task.CompletedTask;
            var dataNascimento = Convert.ToDateTime(
                context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);
            var idade = (DateTime.Now.Date - dataNascimento.Date).Days / 365.25;
            if (idade >= requirement.IdadeMinima)
                context.Succeed(requirement);
            else
                context.Fail();
            return Task.CompletedTask;
        }
    }
}