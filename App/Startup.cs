using System;
using App.Models;
using App.Policies;
using App.Services;
using App.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            // services.Configure<GMailSettings>(Configuration.GetSection(nameof(GMailSettings)));
            // services.AddSingleton<IEmailService, GMailService>();

            // services.Configure<SendinBlueSettings>(Configuration.GetSection(nameof(SendinBlueSettings)));
            // services.AddSingleton<IEmailService, SendinBlueService>();

            services.Configure<SendGridSettings>(Configuration.GetSection(nameof(SendGridSettings)));
            services.AddSingleton<IEmailService, SendGridService>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("AppDbContext")));

            services.AddIdentity<UsuarioModel, IdentityRole<int>>(options =>
                {
                    options.User.RequireUniqueEmail = true; //false
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //idem
                    options.Password.RequireNonAlphanumeric = false; //true
                    options.Password.RequireUppercase = false; //true;
                    options.Password.RequireLowercase = false; //true;
                    options.Password.RequireDigit = false; //true;
                    options.Password.RequiredUniqueChars = 1; //1;
                    options.Password.RequiredLength = 6; //6;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); //5
                    options.Lockout.MaxFailedAccessAttempts = 5; //5
                    options.Lockout.AllowedForNewUsers = true; //true		
                    options.SignIn.RequireConfirmedEmail = false; //false
                    options.SignIn.RequireConfirmedPhoneNumber = false; //false
                    options.SignIn.RequireConfirmedAccount = false; //false
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("MaiorDeIdade", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new MaiorDeIdadeRequirement(18));
                });
            });

            services.AddSingleton<IAuthorizationHandler, MaiorDeIdadeHandler>();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "AppControleUsuarios"; //AspNetCore.Cookies
                options.Cookie.HttpOnly = true; //true
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5); //14 dias
                options.LoginPath = "/Usuario/Login"; // /Account/Login
                options.LogoutPath = "/Home/Index";  // /Account/Logout
                options.AccessDeniedPath = "/Usuario/AcessoRestrito"; // /Account/AccessDenied
                options.SlidingExpiration = true; //true - gera um novo cookie a cada requisição se o cookie estiver com menos de meia vida
                options.ReturnUrlParameter = "returnUrl"; //returnUrl
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<UsuarioModel> userManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            Inicializador.InicializarIdentity(userManager, roleManager);
        }
    }
}
