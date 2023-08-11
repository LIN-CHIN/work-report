using WorkReportAPI;
using WorkReportAPI.RabbitMQ;
using WorkReportAPI.Services;

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

    builder.Services.AddSingleton(appSettings);
    builder.Services.AddTransient<IWorkReportService, WorkReportService>();
    builder.Services.AddTransient<IRabbitMQHelper, RabbitMQHelper>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    //}

    //app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex) 
{
    throw new Exception(ex.ToString());
}
