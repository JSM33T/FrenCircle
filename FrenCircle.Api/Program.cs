using FrenCircle.Api.Extensions;
using FrenCircle.Shared.ConfigModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.Configure<FcConfig>(
    builder.Configuration.GetSection("FcConfig")
);

builder.Services.AddInfrastructureServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
