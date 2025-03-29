namespace FrenCircle.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SmtpConfig>(config.GetSection("SmtpSettings"));

            services.AddScoped<IMailService, SmtpMailService>();

            return services;
        }
    }
}
