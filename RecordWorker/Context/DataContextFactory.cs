using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecordWorker.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordWorker.Context
{
    public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddDbContext<DataContext>(options =>
                        options.UseNpgsql(GetConnectionString()))
                    .BuildServiceProvider();

                return serviceProvider.GetRequiredService<DataContext>();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private string GetConnectionString() 
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var appSettings = config.GetSection("AppSettings")
                .Get<AppSettings>(opt => opt.BindNonPublicProperties = true);

            return appSettings.ConnectionString;
        }
    }
}
