using NSE.Cliente.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();



builder.Services.AddApiConfiguration(builder.Configuration);


var app = builder.Build();

app.UseApiConfiguration(app.Environment);

app.Run();
