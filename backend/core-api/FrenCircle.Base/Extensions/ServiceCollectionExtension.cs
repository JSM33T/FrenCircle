namespace FrenCircle.Base.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddHttpClient();
            return services;
            
        }
    }
}
