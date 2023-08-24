using ElasticsearchAPI.NestClient;
using ElasticsearchAPI.Services;
using ElasticsearchAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddEnvironmentVariables();

var elasticSettings = builder.Configuration
       .GetSection("ElasticSettings")
       .Get<ElasticSettings>(opt => opt.BindNonPublicProperties = true);

builder.Services.AddSingleton(elasticSettings);
builder.Services.AddScoped<INestClientHandler, NestClientHandler>();
builder.Services.AddScoped<IElasticsearchService, ElasticsearchService>();

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
