using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Veterinary.Api;
using Veterinary.Dal.Data;
using Veterinary.Tests.IntegrationTests.Envinroment.Extensions;

namespace Veterinary.Tests.IntegrationTests.Envinroment
{
    public class IntegrationTestStartup : Startup
    {
        private SqliteConnection sqliteConnection;

        public IntegrationTestStartup(IConfiguration configuration) : base(configuration)
        {
            sqliteConnection = new SqliteConnection("Data Source=:memory:");
        }

        public override void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddCustomAuthentication();
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<VeterinaryDbContext>(options =>
                options.UseSqlite(sqliteConnection));
        }

        public override void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers()
                .AddApplicationPart(Assembly.Load("Veterinary.Api"))
                .AddControllersAsServices();
        }
    }
}
