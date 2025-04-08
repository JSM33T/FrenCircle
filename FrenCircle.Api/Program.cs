using FrenCircle.Api.Extensions;
using FrenCircle.Api.Middlewares;
using FrenCircle.Shared.ConfigModels;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


builder.Services.AddOpenApi();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration.GetSection("FcConfig:JwtConfig");
        //options.TokenValidationParameters = new TokenValidationParameters
        //{
        //    ValidateIssuer = false,
        //    ValidateAudience = false,
        //    ValidateLifetime = false,
        //    ValidateIssuerSigningKey = true,
        //    ValidIssuer = config["Issuer"],
        //    ValidAudience = config["Audience"],
        //    ClockSkew = TimeSpan.Zero,
        //    IssuerSigningKey = new SymmetricSecurityKey(
        //        Convert.FromBase64String(config["Key"]!))
        //};

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7233",
            ValidAudience = "https://localhost:7233",
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
        Convert.FromBase64String("N9+K7c9FvS+5eOAvDLX0wT1V5x9eOArdfTP+ml+qxJ4="))
        };
    });


builder.Services.Configure<FcConfig>(
    builder.Configuration.GetSection("FcConfig")
);

builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestTimerMiddleware>();

app.MapControllers();

app.Run();
