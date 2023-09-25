using Microsoft.Extensions.Configuration;
using NSE.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appserrings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
builder.Services.AddWebAppConfig(builder.Configuration);

builder.Services.AddIdentityConfig();

builder.Services.AddRegisterServises(builder.Configuration);

var app = builder.Build();

app.UseWebAppConfig(app.Environment);

app.Run();
