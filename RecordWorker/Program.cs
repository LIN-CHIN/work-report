using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecordWorker;
using RecordWorker.Context;
using RecordWorker.DAOs;
using RecordWorker.RabbitMQ;
using RecordWorker.Settings;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System.Reflection;
using Serilog.Exceptions;
using RecordWorker.Services.WorkReports;
using RecordWorker.Services.LogServices;

try
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    var appSettings = config.GetSection("AppSettings")
        .Get<AppSettings>( opt => opt.BindNonPublicProperties = true);

    var elasticSettings = config.GetSection("ElasticSettings")
       .Get<ElasticSettings>(opt => opt.BindNonPublicProperties = true);

    // 建立 DI 容器
    var serviceProvider = new ServiceCollection()
        .AddDbContext<DataContext>(opt => opt.UseNpgsql(appSettings.ConnectionString))
        .AddSingleton<IConfiguration>(config)
        .AddSingleton(appSettings!)
        .AddSingleton(elasticSettings!)
        .AddSingleton<Application>()
        .AddSingleton<ILogService, LogService>()
        .AddScoped<IWorkReportRecordService, WorkReportRecordService>()
        .AddScoped<IWorkReportRecordDAO, WorkReportRecordDAO>()
        .AddScoped<IRabbitMQHelper, RabbitMQHelper>()
        .AddLogging(builder =>
        {
            builder.AddSerilog();
        })
        .BuildServiceProvider();

    // Entity Framework migrate on startup
    using (var scope = serviceProvider.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }

    //設定Log
    ConfigureLogging(elasticSettings!.Url);

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
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
        .WriteTo.Console()
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