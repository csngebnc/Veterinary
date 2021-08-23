using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Model.Entities;

namespace Veterinary.Dal.Data
{
    public class DatabaseInitializer
    {
        private static ILogger<DatabaseInitializer> logger;
        public static async Task SeedDatabase(IServiceProvider services, VeterinaryDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<VeterinaryUser> userManager)
        {
            logger = services.GetRequiredService<ILogger<DatabaseInitializer>>();
            logger.LogInformation("Start seeding the database.");
            await TryCreateRolesAsync(roleManager);
            var users = await TryCreateUsersAsync(context);
            await AddRoleToUsers(userManager, users);
            logger.LogInformation("End of seeding the database.");
        }

        private static async Task TryCreateRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            if ((await roleManager.Roles.CountAsync()) > 0)
            {
                logger.LogInformation("Roles are already seeded.");
                return;
            }

            logger.LogInformation("Start seeding roles.");
            await roleManager.CreateAsync(new IdentityRole<Guid>("ManagerDoctor"));
            await roleManager.CreateAsync(new IdentityRole<Guid>("NormalDoctor"));
            await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            logger.LogInformation("SEED COMPLETED: roles");
        }
        private static async Task<ICollection<VeterinaryUser>> TryCreateUsersAsync(VeterinaryDbContext context)
        {
            if ((await context.Users.CountAsync()) > 0)
            {
                logger.LogInformation("Users are already seeded.");
                return new List<VeterinaryUser>();
            }

            logger.LogInformation("Start seeding users.");
            var users = new List<VeterinaryUser>
            {
                new VeterinaryUser{ UserName = "manager@veterinary.hu", Email = "manager@veterinary.hu" },
                new VeterinaryUser{ UserName = "doctor@veterinary.hu", Email = "doctor@veterinary.hu" },
                new VeterinaryUser{ UserName = "user1@veterinary.hu", Email = "user1@veterinary.hu" },
                new VeterinaryUser{ UserName = "user2@veterinary.hu", Email = "user2@veterinary.hu" }
            };

            PasswordHasher<VeterinaryUser> passwordHasher = new PasswordHasher<VeterinaryUser>();
            foreach (var user in users)
            {
                user.PasswordHash = passwordHasher.HashPassword(user, "Aa1234.");
                user.NormalizedUserName = user.UserName.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();
                user.SecurityStamp = Guid.NewGuid().ToString();
            }

            await context.AddRangeAsync(users);
            await context.SaveChangesAsync();
            logger.LogInformation("SEED COMPLETED: users");

            return users;
        }

        private static async Task AddRoleToUsers(UserManager<VeterinaryUser> userManager, ICollection<VeterinaryUser> users)
        {
            if(users.Count == 0)
            {
                logger.LogInformation("User roles are already seeded.");
                return;
            }

            logger.LogInformation("Start seeding user roles.");
            for (int i = 0; i < users.Count; i++)
            {
                if (i == 0)
                {
                    await userManager.AddToRoleAsync(users.ElementAt(i), "ManagerDoctor");
                }
                else if (i == 1)
                {
                    await userManager.AddToRoleAsync(users.ElementAt(i), "NormalDoctor");
                }
                else
                {
                    await userManager.AddToRoleAsync(users.ElementAt(i), "User");
                }
            }
            logger.LogInformation("SEED COMPLETED: user roles");
        }

    }
}
