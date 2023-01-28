using Spents.EventSourcing.CrossCuting.Extensions;
using Spents.EventSourcing.CrossCuting.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hosting, config) =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    config
        .SetBasePath(currentDirectory)
        .AddJsonFile($"{currentDirectory}/appsettings.json");
});

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

// Add services to the container.
builder.Services
    .AddKafka(applicationSettings.KafkaSettings)
    .AddMongo(applicationSettings.MongoSettings)
    .AddLoggingDependency()
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();