using Domain.Services;
using Domain.Services.IServices;
using Infrastructure.Business.SERVICES;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var apiKey = configuration["ApiSettings:ApiKey"] ?? throw new Exception("ApiKey is missing");
var apiUrl = configuration["ApiSettings:ApiUrl"] ?? throw new Exception("ApiUrl is missing");

builder.Services.AddHttpClient();

// Регистрация сервисов напрямую через DI
builder.Services.AddScoped<IGenerateCongratilation, GenerateCongratulations>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new GenerateCongratulations(apiKey, apiUrl, httpClient);
});

builder.Services.AddScoped<IProxyApiClient, ProxyApiClient>();
builder.Services.AddScoped<IAddNewCongratulationsService, AddNewCongratulationsService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddSingleton<Serilog.ILogger>(Log.Logger); 

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();