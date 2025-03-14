
using Domain.Services;
using Domain.Services.IServices;
using Infrastructure.Business.SERVICES;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var apiKey = configuration["ApiSettings:ApiKey"];
var apiUrl = configuration["ApiSettings:ApiUrl"];

// Регистрация зависимостей
builder.Services.AddScoped<IGenerateCongratilation>(provider =>
{
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new GenerateCongratulations(apiKey, apiUrl, httpClient);
});

builder.Services.AddScoped<IGenerateCongratilation, GenerateCongratulations>();
builder.Services.AddScoped<IProxyApiClient, ProxyApiClient>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IAddNewCongratulationsService, AddNewCongratulationsService>();
builder.Services.AddControllers();

builder.Services.AddOpenApi();
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() 
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

