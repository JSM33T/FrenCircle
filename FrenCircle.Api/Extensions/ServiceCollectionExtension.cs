using FrenCircle.Application;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Infra.Background;
using FrenCircle.Infra.Dapper;
using FrenCircle.Infra.MailService;
using FrenCircle.Infra.MailService.SmtpMail;
using FrenCircle.Infra.Telegram;
using FrenCircle.Infra.Token;
using FrenCircle.Repositories;

namespace FrenCircle.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IMailService, SmtpMailService>();

            services.AddScoped<IChangeLogRepository, ChangeLogRepository>();


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IChangeLogService, ChangeLogService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IDapperFactory, DapperFactory>();

            services.AddSingleton<IDispatcher, Dispatcher>();
            services.AddScoped<IJobHistoryRepository, JobHistoryRepository>();
            services.AddHostedService<JobWorker>();

            services.AddHttpClient<ITelegramService, TelegramService>();

            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IProfileRepository, ProfileRepository>();

            return services;
        }
    }
}
