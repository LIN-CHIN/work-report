using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishWorker;

try
{
    var config = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .Build();

    // 建立 DI 容器
    var serviceProvider = new ServiceCollection()
        .AddSingleton<IConfiguration>(config)
        .AddSingleton<Application>()
        .BuildServiceProvider();

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
