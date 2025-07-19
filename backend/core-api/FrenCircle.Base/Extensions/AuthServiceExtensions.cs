using FrenCircle.Data;
using FrenCircle.Data.Repositories;
using FrenCircle.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Text;

namespace FrenCircle.Base.Extensions
{
    public static class AuthServiceExtensions
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
        {
         

            // Add Entity Framework DbContext
            services.AddDbContext<AuthDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                // Configure for PostgreSQL (you can change this based on your database)
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
                });

                // Enable sensitive data logging in development
                if (configuration.GetValue<bool>("EnableSensitiveDataLogging"))
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            // Register repositories
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IOAuthService, OAuthService>();
            services.AddHttpClient<IOAuthService, OAuthService>();

            // Add authentication and JWT support
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    var jwtSettings = configuration.GetSection("JwtSettings");
                    var secretKey = jwtSettings["SecretKey"];

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    var googleConfig = configuration.GetSection("Authentication:Google");
                    options.ClientId = googleConfig["ClientId"]!;
                    options.ClientSecret = googleConfig["ClientSecret"]!;
                    options.SaveTokens = true;
                    
                    // Request additional scopes
                    options.Scope.Add("email");
                    options.Scope.Add("profile");
                });

            // Add authorization
            services.AddAuthorization(options =>
            {
                // Default policy requires authentication
                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // Add role-based policies
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ModeratorOrAdmin", policy => policy.RequireRole("Moderator", "Admin"));
                options.AddPolicy("UserAccess", policy => policy.RequireRole("User", "Moderator", "Admin"));

                // Add permission-based policies
                options.AddPolicy("ManageUsers", policy => policy.RequireClaim("permission", "users.manage"));
                options.AddPolicy("SystemAdmin", policy => policy.RequireClaim("permission", "system.admin"));
            });

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AuthPolicy", policy =>
                {
                    policy.WithOrigins(configuration.GetSection("Auth:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:4200" })
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAuthServices(this IApplicationBuilder app)
        {
            app.UseCors("AuthPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
