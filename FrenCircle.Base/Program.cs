using FluentValidation;
using FluentValidation.AspNetCore;
using FrenCircle.Base.Extensions;
using FrenCircle.Base.Middlewares;
using FrenCircle.Entities;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Async(a => a.File($"Logs/log.txt", rollingInterval: RollingInterval.Hour))
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Host.UseSerilog();

var key = Encoding.ASCII.GetBytes("iureowtueorituowierutoi4354======");

builder.Services.Configure<FcConfig>(builder.Configuration.GetSection("FCConfig"));

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
        ValidIssuer = "https://frencircle.com",
        ValidAudience = "https://frencircle.com",
        RoleClaimType = ClaimTypes.Role,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

builder.ConfigureSignalRServices();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}



builder.Services.AddSingleton<IRateLimiter, RateLimiter>();
builder.Services.AddScoped<IDapperFactory, DapperFactory>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddHttpClient<ITelegramService, TelegramService>();
builder.Services.AddScoped<IClaimsService, ClaimsService>();

builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("FrenCircle.Validators"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseSignalRConfiguration();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<FcRequestMiddleware>();


app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();