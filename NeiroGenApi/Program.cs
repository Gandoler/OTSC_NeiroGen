using System.Net.Http.Headers;
using Domain.Services;
using Domain.Services.IServices;
using Infrastructure.Business.SERVICES;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

});


//          для обычного запуска

var configuration = builder.Configuration;
var apiKey = configuration["ApiSettings:ApiKey"] ?? throw new Exception("ApiKey is missing");
var dbProxy = configuration["ApiSettings:DbProxy"] ?? throw new Exception("DbProxy is missing");
var apiUrl = configuration["ApiSettings:ApiUrl"] ?? throw new Exception("ApiUrl is missing");


//        для докера
// var apiKey = Environment.GetEnvironmentVariable("ApiKey") ?? throw new Exception("ApiKey is missing");
// var dbProxy = Environment.GetEnvironmentVariable("DbProxy") ?? throw new Exception("DbProxy is missing");
// var apiUrl = Environment.GetEnvironmentVariable("ApiUrl") ?? throw new Exception("ApiUrl is missing");


// HttpClient для Proxy API (работает с `DbProxy`)
builder.Services.AddHttpClient("ProxyApiClient", client =>
{
    client.BaseAddress = new Uri(dbProxy);
});

// HttpClient для генерации поздравлений (работает с `apiUrl` и `apiKey`)
builder.Services.AddHttpClient("GenerateCongratulations", client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
});

// Регистрация `GenerateCongratulations` с правильным HttpClient
builder.Services.AddScoped<IGenerateCongratilation>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("GenerateCongratulations");
    return new GenerateCongratulations(apiKey, apiUrl, httpClient);
});

// Регистрация `ProxyApiClient` с `DbProxy`
builder.Services.AddScoped<IProxyApiClient>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("ProxyApiClient");
    return new ProxyApiClient(httpClient);
});

// Регистрация основного сервиса
builder.Services.AddScoped<IAddNewCongratulationsService, AddNewCongratulationsService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// Логирование через Serilog
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
    app.MapControllers();
}
app.MapOpenApi();
app.MapControllers();
app.UseHttpsRedirection();


Log.Information($"Apikey: {apiKey}");
Log.Information($"DbProxy: {dbProxy}");
Log.Information($"ApiUrl: {apiUrl}");

app.Run();
