using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;

namespace Veterinary.Tests.IntegrationTests.Envinroment
{
    public class VeterinaryFactory : WebApplicationFactory<IntegrationTestStartup>
    {
        public Guid UserIdManager { get => Guid.Parse("85b4a2a3-aa22-40e4-9b91-c72e96ab8e07"); }
        public Guid UserIdDoctor { get => Guid.Parse("85b4a2a3-aa22-40e4-9b91-c72e96ab8e06"); }
        public Guid UserIdUser { get => Guid.Parse("85b4a2a3-aa22-40e4-9b91-c72e96ab8e08"); }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHost => webHost.UseStartup<IntegrationTestStartup>());
        }

        public async Task SeedInitOptions()
        {
            using var scope = Server.Services.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<VeterinaryDbContext>();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var managerRole = new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "ManagerDoctor",
                NormalizedName = "MANAGERDOCTOR",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var doctorRole = new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "NormalDoctor",
                NormalizedName = "NORMALDOCTOR",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };
            var userRole = new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            context.Roles.Add(managerRole);
            context.Roles.Add(doctorRole);
            context.Roles.Add(userRole);
            await context.SaveChangesAsync();            

            var users = new List<VeterinaryUser>
            {
                new VeterinaryUser
                {
                    Id = UserIdManager,
                    UserName = "manager",
                    Email = "manager@veterinary.hu"
                },
                new VeterinaryUser
                {
                    Id = UserIdUser,
                    UserName = "user",
                    Email = "user@veterinary.hu"
                },
                new VeterinaryUser
                {
                    Id = UserIdDoctor,
                    UserName = "doctor",
                    Email = "doctor@veterinary.hu"
                },
            };

            PasswordHasher<VeterinaryUser> passwordHasher = new PasswordHasher<VeterinaryUser>();
            foreach (var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "Aa1234.");
                user.NormalizedUserName = user.UserName.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();
                user.SecurityStamp = Guid.NewGuid().ToString();
            }

            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            var managerUserRole = new IdentityUserRole<Guid>
            {
                UserId = UserIdManager,
                RoleId = managerRole.Id
            };
            var doctorUserRole = new IdentityUserRole<Guid>
            {
                UserId = UserIdDoctor,
                RoleId = doctorRole.Id
            };
            var userUserRole = new IdentityUserRole<Guid>
            {
                UserId = UserIdUser,
                RoleId = userRole.Id
            };

            context.UserRoles.Add(managerUserRole);
            context.UserRoles.Add(doctorUserRole);
            context.UserRoles.Add(userUserRole);
            await context.SaveChangesAsync();
        }
    }
}
