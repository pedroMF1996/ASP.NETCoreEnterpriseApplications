using NSE.Pedido.API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

builder.Services.AddApiConfiguration(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseApiConfiguration(app.Environment);

app.MapControllers();

app.Run();
