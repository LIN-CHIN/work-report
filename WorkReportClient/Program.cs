using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkReportClient;
using WorkReportClient.Services;

try
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    var appSettings = config.GetSection("AppSettings")
        .Get<AppSettings>(opt => opt.BindNonPublicProperties = true);

    // 建立 DI 容器
    var serviceProvider = new ServiceCollection()
        .AddSingleton<Application>()
        .AddSingleton(appSettings!)
        .AddTransient<IWorkReportService, WorkReportService>()
        .AddHttpClient()
        .BuildServiceProvider();

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
} 
catch (Exception ex) 
{
    Console.WriteLine(ex.ToString());
}
