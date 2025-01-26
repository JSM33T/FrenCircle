using FrenCircle.Base.Hubs;

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
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
            });
        }

        public static void UseSignalRConfiguration(this WebApplication app)
        {
            app.UseCors("AllowAngularApp");
            app.MapHub<AudioStreamHub>("/audioStreamHub");
        }
    }
}
