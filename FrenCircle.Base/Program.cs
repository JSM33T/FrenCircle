using FluentValidation;
using FluentValidation.AspNetCore;
using FrenCircle.Base.Extensions;
using FrenCircle.Base.Middlewares;
using FrenCircle.Entities;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes("iureowtueorituowierutoi4354======");

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "www.frencircle.com",
        ValidAudience = "www.frencircle.com",
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

builder.ConfigureSignalRServices();

builder.Services.Configure<FcConfig>(builder.Configuration.GetSection("FCConfig"));

builder.Services.AddSingleton<IRateLimiter, RateLimiter>();

builder.Services.AddScoped<IDapperFactory, DapperFactory>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<FcRequestMiddleware>();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseSignalRConfiguration();

app.Run();
