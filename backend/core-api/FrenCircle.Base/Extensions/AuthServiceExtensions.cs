using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FrenCircle.Data;
using FrenCircle.Data.Repositories;

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
            services.AddScoped<IAuthRepository, AuthRepository>();

            // Add authentication and JWT support
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["Auth:Authority"];
                    options.RequireHttpsMetadata = !configuration.GetValue<bool>("Auth:DisableHttpsRequirement");
                    options.Audience = configuration["Auth:Audience"];
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
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
