using CalculateWorker;
using CalculateWorker.CacheServices;
using CalculateWorker.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

try
{
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables()
        .Build();

    var appSettings = config.GetSection("AppSettings")
            .Get<AppSettings>(opt => opt.BindNonPublicProperties = true);

    // 建立 DI 容器
    var serviceProvider = new ServiceCollection()
        .AddSingleton<IConfiguration>(config)
        .AddSingleton<Application>()
        .AddSingleton(appSettings!)
        .AddSingleton<ICacheService, CacheService>()
        .AddScoped<IRabbitMQHelper, RabbitMQHelper>()
        .BuildServiceProvider();

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
