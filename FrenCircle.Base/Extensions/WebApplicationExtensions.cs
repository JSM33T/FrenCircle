namespace FrenCircle.Base.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureSignalRServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddSignalR();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                    policy.SetIsOriginAllowed(origin =>
                        origin == "http://localhost:4200" ||
                        origin == "https://preview.frencircle.com" ||
                        origin.StartsWith("https://frencircle.com"))
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
            });
        }

        public static void UseSignalRConfiguration(this WebApplication app)
        {
            app.UseCors("AllowAngularApp");
        }
    }
}
