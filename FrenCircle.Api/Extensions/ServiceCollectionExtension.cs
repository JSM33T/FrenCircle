using FrenCircle.Application;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Infra.Dapper;
using FrenCircle.Infra.MailService;
using FrenCircle.Infra.MailService.SmtpMail;
using FrenCircle.Repositories;

namespace FrenCircle.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SmtpConfig>(config.GetSection("SmtpSettings"));

            services.AddScoped<IMailService, SmtpMailService>();

            services.AddScoped<IChangeLogRepository, ChangeLogRepository>();


            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IChangeLogService, ChangeLogService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddSingleton<IDapperFactory, DapperFactory>();


            return services;
        }
    }
}
