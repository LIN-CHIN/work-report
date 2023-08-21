using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;
using WorkReportClient;
using WorkReportClient.Settings;
using Serilog.Exceptions;
using WorkReportClient.Services.WorkReports;
using WorkReportClient.Services.LogServices;
using Serilog.Context;
using WorkReportClient.AppLogs;

try
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    //App Settings
    var appSettings = config.GetSection("AppSettings")
        .Get<AppSettings>(opt => opt.BindNonPublicProperties = true);

    var elasticSettings = config.GetSection("ElasticSettings")
        .Get<ElasticSettings>(opt => opt.BindNonPublicProperties = true);

    //建立DI容器
    var serviceProvider = new ServiceCollection()
        .AddSingleton<Application>()
        .AddSingleton<IConfiguration>(config)
        .AddSingleton(appSettings!)
        .AddSingleton<ILogService, LogService>()
        .AddTransient<IWorkReportService, WorkReportService>()
        .AddHttpClient()
        .AddLogging( builder => 
        {
            builder.AddSerilog();
        })
        .BuildServiceProvider();

    //設定Log
    ConfigureLogging(elasticSettings!.Url);

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
} 
catch (Exception ex) 
{
    Log.Error(ex.ToString());
    Console.WriteLine(ex.ToString());
}

void ConfigureLogging(string elasticUrl)
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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