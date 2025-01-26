using FluentValidation;
using FluentValidation.AspNetCore;
using FrenCircle.Base.Extensions;
using FrenCircle.Base.Middlewares;
using FrenCircle.Entities;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.ConfigureSignalRServices();

builder.Services.Configure<FCConfig>(builder.Configuration.GetSection("FCConfig"));

builder.Services.AddSingleton<IRateLimiter, RateLimiter>();

builder.Services.AddScoped<IDapperFactory, DapperFactory>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();

#region Fluent Validatoins
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("FrenCircle.Validators"));
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<FcRequestMiddleware>();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseSignalRConfiguration();

app.Run();
