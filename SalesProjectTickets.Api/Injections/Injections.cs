using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Application.Validations;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Backgrounds;
using SalesProjectTickets.Infrastructure.Provider;
using SalesProjectTickets.Infrastructure.Repositories;
using System.Text;

namespace SalesProjectTickets.Api.Injections
{
    public class Injections
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(typeof(UserValidations).Assembly);
            services.AddValidatorsFromAssemblyContaining<UserValidations>();

            services.AddScoped<IServiceTickets<Tickets>, TicketsServices>();
            services.AddScoped<IRepoTickets<Tickets>, TicketsRepo>();

            services.AddHostedService<TimerExpirationTickets>();

            services.AddScoped<IServiceLogin<LoginUsers>, LoginServices>();
            services.AddScoped<IRepoLogin<LoginUsers>, UserReposLogin>();
            services.AddSingleton<TokenProvider>();
            services.AddHttpContextAccessor();

            services.AddAuthorization();

            //configuracion
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorizationBuilder()
                .AddPolicy("SuperUsuario", policy => policy.RequireClaim("Permissions", "SuperUsuario"))
                .AddPolicy("Administrador", policy => policy.RequireClaim("Permissions", "Administrador"))
                .AddPolicy("Consumidor", policy => policy.RequireClaim("Permissions", "Consumidor"))
                .AddPolicy("AdminAndConsu", policy => policy.RequireClaim("Permissions", "Administrador", "Consumidor"));

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });
        }
    }
}
