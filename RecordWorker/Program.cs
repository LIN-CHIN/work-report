using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecordWorker;
using RecordWorker.Context;
using RecordWorker.DAOs;
using RecordWorker.Services;

try
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    var appSettings = config.GetSection("AppSettings")
        .Get<AppSettings>( opt => opt.BindNonPublicProperties = true);

    // 建立 DI 容器
    var serviceProvider = new ServiceCollection()
        .AddDbContext<DataContext>(opt => opt.UseNpgsql(appSettings.ConnectionString))
        .AddSingleton<IConfiguration>(config)
        .AddSingleton(appSettings!)
        .AddSingleton<Application>()
        .AddScoped<IWorkReportRecordService, WorkReportRecordService>()
        .AddScoped<IWorkReportRecordDAO, WorkReportRecordDAO>()
        .BuildServiceProvider();

    // Entity Framework migrate on startup
    using (var scope = serviceProvider.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate();
    }

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
