using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
using WorkReportAPI.AppLogs;
using WorkReportAPI.Middlewares;
using WorkReportAPI.RabbitMQ;
using WorkReportAPI.Services;
using WorkReportAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

try
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Configuration.AddEnvironmentVariables();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var appSettings = builder.Configuration
        .GetSection("AppSettings")
        .Get<AppSettings>(opt => opt.BindNonPublicProperties = true);

    var elasticSettings = builder.Configuration
       .GetSection("ElasticSettings")
       .Get<ElasticSettings>(opt => opt.BindNonPublicProperties = true);

    //Dependency Injection
    builder.Services.AddSingleton(appSettings!);
    builder.Services.AddSingleton(elasticSettings!);
    builder.Services.AddSingleton<ILogService, LogService>();
    builder.Services.AddTransient<IWorkReportService, WorkReportService>();
    builder.Services.AddTransient<IRabbitMQHelper, RabbitMQHelper>();

    //設定Log
    ConfigureLogging(elasticSettings!.Url);
    builder.Host.UseSerilog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    //}

    //app.UseHttpsRedirection();
    
    app.UseMiddleware<LoggingMiddleware>();

    app.UseAuthorization();
    app.MapControllers();


    app.Run();

}
catch (Exception ex) 
{
    Log.Error($"系統發生錯誤：{ex}" ) ;
    throw new Exception(ex.ToString());
}


void ConfigureLogging(string elasticUrl)
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Elasticsearch(ConfigureElasticSink(elasticUrl))
        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticSink(string elasticUrl)
{
    return new ElasticsearchSinkOptions(new Uri(elasticUrl))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"[WorkReport]-{Assembly.GetExecutingAssembly().GetName().Name.Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd}"
    };
}