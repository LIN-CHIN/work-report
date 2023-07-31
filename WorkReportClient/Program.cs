using Microsoft.Extensions.DependencyInjection;
using WorkReportClient;
using WorkReportClient.Services;

try
{
    // 建立 DI 容器
    var serviceProvider = new ServiceCollection()
        .AddTransient<Application>()
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
