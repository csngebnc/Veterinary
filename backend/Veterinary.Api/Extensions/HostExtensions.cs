using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;

namespace Veterinary.Api.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TContext>();

                context.Database.Migrate();
            }

            return host;
        }

        public static async Task<IHost> SeedDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<VeterinaryDbContext>();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var userManager = services.GetRequiredService<UserManager<VeterinaryUser>>();

                await DatabaseInitializer.SeedDatabase(services, context, roleManager, userManager);
            }

            return host;
        }
    }
}
