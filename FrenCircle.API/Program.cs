//using Jsm33t.Repositories;
//using Jsm33t.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using FrenCircle.API.Middlewares;
using FrenCircle.Entities.Shared;
using FrenCircle.Repositories;
using FrenCircle.Services;
using Jsm33t.Entities.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

#region Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Async(a => a.File($"Logs/log.txt", rollingInterval: RollingInterval.Hour))
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
#endregion

#region Fluent Validatoins
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
//builder.Services.AddValidatorsFromAssembly(Assembly.Load("FC.Validators"));
#endregion

builder.Services.AddControllers();

#region Config with Change Tracking

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}

#endregion

var fCConfigSection = builder.Configuration.GetSection("fCConfig");
FCConfig fCConfig = builder.Configuration.GetSection("fCConfig").Get<FCConfig>();

builder.Services.Configure<FCConfig>(fCConfigSection);

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(fCConfig?.ConnectionString));


#region Repositories

builder.Services.AddScoped<IGlobalRepository, GlobalRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion

#region Services

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IUserService, UserService>();

#endregion

#region Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = fCConfig?.JwtSettings.ValidIssuer,
            ValidAudience = fCConfig?.JwtSettings.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(fCConfig.JwtSettings.IssuerSigningKey))
        };
    });

#endregion

builder.Services.AddMemoryCache();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});


builder.Services.AddHttpClient();

//builder.Services.AddDataProtection()
//    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.WebRootPath, "keys")))
//    .SetApplicationName("AlmondcoveApp");

var rateLimitingOptions = new RateLimitingConfig();
builder.Configuration.GetSection("RateLimiting").Bind(rateLimitingOptions);


#region rateLimiter

var rateLimitSettings = builder.Configuration.GetSection("RateLimiting").Get<RateLimitingConfig>();

// Register the global rate limiter
builder.Services.AddRateLimiter(options =>
{
    if (rateLimitSettings?.Global != null)
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", _ =>
                new FixedWindowRateLimiterOptions
                {
                    PermitLimit = rateLimitSettings.Global.PermitLimit,
                    Window = rateLimitSettings.Global.Window,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = rateLimitSettings.Global.QueueLimit
                }));
    }

    // Register route-specific limiters
    if (rateLimitSettings?.Routes != null)
    {
        foreach (var route in rateLimitSettings.Routes)
        {
            options.AddPolicy(route.Key, partition =>
                RateLimitPartition.GetFixedWindowLimiter(partition, _ =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = route.Value.PermitLimit,
                        Window = route.Value.Window,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = route.Value.QueueLimit
                    }));
        }
    }
});
#endregion


builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(o => o.AddPolicy("OpenPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("OpenPolicy");
app.UseHttpsRedirection();
app.UseMiddleware<FCValidationMiddleware>();
app.UseStaticFiles();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();