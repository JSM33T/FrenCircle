using API.Base.Middlewares;
using API.Contracts.Repositories;
using API.Contracts.Services;
using API.Entities.Shared;
using API.Infra;
using API.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Serilog;
using System.Reflection;
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
builder.Services.AddValidatorsFromAssembly(Assembly.Load("API.Validators"));
#endregion

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}

var Jsm33tConfigSection = builder.Configuration.GetSection("Jsm33tConfig");
var Jsm33tConfig = builder.Configuration.GetSection("Jsm33tConfig").Get<Jsm33tConfig>();

builder.Services.Configure<Jsm33tConfig>(Jsm33tConfigSection);

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new DbConnectionFactory(Jsm33tConfig.ConnectionString)
);

//Register repositories
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IDataService, DataService>();

//Register services
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IRateLimitService, RateLimitService>();

#region Auth
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = Jsm33tConfig?.JwtSettings.ValidIssuer,
//            ValidAudience = Jsm33tConfig?.JwtSettings.ValidAudience,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jsm33tConfig.JwtSettings.IssuerSigningKey))
//        };
//    });
#endregion


builder.Services.AddMemoryCache();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});


builder.Services.AddHttpClient();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.WebRootPath, "keys")))
    .SetApplicationName("AlmondcoveApp");

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

app.UseMiddleware<AcValidationMiddleware>();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();