using FrenCircle.Base.Extensions;
using FrenCircle.Base.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddUserServices(builder.Environment);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<RequestTimerMiddleware>();
app.UseMiddleware<FcRequestMiddleware>();

app.MapControllers();

app.Run();
