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



app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<RequestTimerMiddleware>();
app.UseMiddleware<FcRequestMiddleware>();
app.UseAuthentication();
app.UseAuthServices();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html", "text/html");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();