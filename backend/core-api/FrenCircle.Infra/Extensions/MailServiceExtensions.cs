using FrenCircle.Contracts.Mail;
using FrenCircle.Infra.Background;
using FrenCircle.Infra.Repositories;
using FrenCircle.Infra.Repositories.Interfaces;
using FrenCircle.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrenCircle.Infra.Extensions
{
    public static class MailServiceExtensions
    {
        /// <summary>
        /// Adds mail services to the dependency injection container
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration instance</param>
        /// <param name="enableBackgroundProcessing">Whether to enable background email processing</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMailServices(this IServiceCollection services, IConfiguration configuration, bool enableBackgroundProcessing = false)
        {
            // Configure email settings from configuration
            
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings").Bind);
            
            // Register mail repository
            services.AddScoped<IMailRepository, MailRepository>();
            
            // Add background processing if requested
            if (enableBackgroundProcessing)
            {
                services.AddSingleton<EmailQueue>();
                services.AddScoped<IEmailQueue>(provider => provider.GetRequiredService<EmailQueue>());
                services.AddHostedService<EmailProcessingService>();
            }
            
            return services;
        }

        /// <summary>
        /// Adds mail services with custom email settings
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="emailSettings">The email settings</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMailServices(this IServiceCollection services, EmailSettings emailSettings)
        {
            // Configure email settings directly
            services.Configure<EmailSettings>(options =>
            {
                options.SmtpHost = emailSettings.SmtpHost;
                options.SmtpPort = emailSettings.SmtpPort;
                options.EnableSsl = emailSettings.EnableSsl;
                options.Username = emailSettings.Username;
                options.Password = emailSettings.Password;
                options.FromEmail = emailSettings.FromEmail;
                options.FromName = emailSettings.FromName;
                options.TimeoutSeconds = emailSettings.TimeoutSeconds;
                options.EnableLogging = emailSettings.EnableLogging;
            });
            
            // Register mail repository
            services.AddScoped<IMailRepository, MailRepository>();
            
            return services;
        }

        /// <summary>
        /// Adds mail services with configuration action
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configureOptions">Action to configure email settings</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddMailServices(this IServiceCollection services, Action<EmailSettings> configureOptions)
        {
            // Configure email settings with action
            services.Configure(configureOptions);
            
            // Register mail repository
            services.AddScoped<IMailRepository, MailRepository>();
            
            return services;
        }
    }
}
