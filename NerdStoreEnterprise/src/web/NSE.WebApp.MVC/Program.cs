using NSE.WebApp.MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebAppConfig();

builder.Services.AddIdentityConfig();

var app = builder.Build();

app.UseWebAppConfig(app.Environment);

app.Run();
