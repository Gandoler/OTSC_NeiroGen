
using Domain.Services;
using Domain.Services.IServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IGenerateCongratilation, GenerateCongratulations>();
builder.Services.AddScoped<IProxyApiClient, ProxyApiClient>();

builder.Services.AddScoped<IAddNewCongratulationsService, IAddNewCongratulationsService>();
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

