using FrenCircle.Base.Extensions;
using FrenCircle.Base.Middlewares;
using FrenCircle.Contracts.Auth;
using FrenCircle.Contracts.Shared;

var builder = WebApplication.CreateBuilder(args);

var fcConfig = builder.Configuration.GetSection("FCConfig").Get<FcConfig>();
builder.Services.AddSingleton(fcConfig);


builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddSingleton(sp =>
             sp.GetRequiredService<IConfiguration>().GetSection("JwtSettings").Get<JwtSettings>()
         );
// Add authentication services
builder.Services.AddAuthServices(builder.Configuration);

builder.Services.AddUserServices(builder.Environment, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Use authentication services (must be before UseAuthorization)
app.UseAuthServices();

app.UseMiddleware<RequestTimerMiddleware>();
app.UseMiddleware<FcRequestMiddleware>();

app.MapControllers();

app.Run();
