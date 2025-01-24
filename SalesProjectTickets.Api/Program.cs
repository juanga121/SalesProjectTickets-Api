using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Application.Services;
using SalesProjectTickets.Application.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Domain.Interfaces.Repositories;
using SalesProjectTickets.Infrastructure.Contexts;
using SalesProjectTickets.Infrastructure.Provider;
using SalesProjectTickets.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.Hosting;
using FluentAssertions.Common;
using SalesProjectTickets.Domain.Interfaces;
using SalesProjectTickets.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("ConexionDB");
builder.Services.AddDbContext<ContextsDaBa>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssembly(typeof(UserValidations).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<UserValidations>();

builder.Services.AddScoped<IServiceLogin<LoginUsers>, LoginServices>();
builder.Services.AddScoped<IRepoLogin<LoginUsers>, UserReposLogin>();
builder.Services.AddSingleton<TokenProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization();
//configuracion
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("SuperUsuario", policy => policy.RequireClaim("Permissions", "SuperUsuario"))
    .AddPolicy("Administrador", policy => policy.RequireClaim("Permissions", "Administrador"))
    .AddPolicy("Consumidor", policy => policy.RequireClaim("Permissions", "Consumidor"))
    .AddPolicy("AdminAndConsu", policy => policy.RequireClaim("Permissions", "Administrador", "Consumidor"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
