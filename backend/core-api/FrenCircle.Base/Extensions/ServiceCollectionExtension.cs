using FrenCircle.Infra.Extensions;

namespace FrenCircle.Base.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration configuration)
        {
            services.AddHttpClient();
            
            // Add mail services
            services.AddMailServices(configuration);
            
            return services;
        }
    }
}
