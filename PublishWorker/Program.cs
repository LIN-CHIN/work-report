using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PublishWorker;
using PublishWorker.RabbitMQ;

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
        .AddScoped<IRabbitMQHelper, RabbitMQHelper>()
        .BuildServiceProvider();

    Application application = serviceProvider.GetRequiredService<Application>();
    application.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
