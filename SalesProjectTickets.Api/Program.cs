using Microsoft.EntityFrameworkCore;
using SalesProjectTickets.Infrastructure.Contexts;
using SalesProjectTickets.Api.Injections;
using CloudinaryDotNet;
using SalesProjectTickets.Application.Interfaces;
using SalesProjectTickets.Domain.Entities;
using SalesProjectTickets.Infrastructure.Backgrounds;
using SalesProjectTickets.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging();

var connectionString = builder.Configuration.GetConnectionString("ConexionDB");
builder.Services.AddDbContext<ContextsDaBa>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Injections.ConfigureServices(builder.Services, builder.Configuration);

var cloudiarySettings = builder.Configuration.GetSection("Cloudinary");

var cloud = new Cloudinary(new Account(
    cloudiarySettings["CloudName"],
    cloudiarySettings["ApiKey"],
    cloudiarySettings["ApiSecret"]
));

builder.Services.AddSingleton(cloud);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("DefaultPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
