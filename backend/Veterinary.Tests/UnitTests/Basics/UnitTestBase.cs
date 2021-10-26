using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Services;
using Veterinary.Dal.Data;
using Veterinary.Domain.Entities;

namespace Veterinary.Tests.UnitTests.Basics
{
    public class UnitTestBase : IDisposable
    {
        protected readonly SqliteConnection connection;
        protected VeterinaryDbContext veterinaryDbContext;
        protected readonly IIdentityService identityServiceManager;
        protected readonly IIdentityService identityServiceDoctor;
        protected readonly IIdentityService identityServiceUser;
        protected readonly UnitTestRepositories mockedRepositories;

        protected readonly Guid UserIdManager;
        protected readonly Guid UserIdUser;
        protected readonly Guid UserIdDoctor;

        public UnitTestBase()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<VeterinaryDbContext>()
                .UseSqlite(connection)
                .Options;

            UserIdManager = Guid.Parse("85b4a2a3-aa22-40e4-9b91-c72e96ab8e07");
            UserIdDoctor = Guid.Parse("85b4a2a3-aa22-40e4-9b91-c72e96ab8e06");
            UserIdUser = Guid.Parse("85b4a2a3-aa22-40e4-9b91-c72e96ab8e08");

            var mockIdentityServiceManager = new Mock<IIdentityService>();
            mockIdentityServiceManager.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "ManagerDoctor")).Returns(Task.FromResult(true));
            mockIdentityServiceManager.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "NormalDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceManager.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "User")).Returns(Task.FromResult(false));
            mockIdentityServiceManager.Setup(x => x.IsInRoleAsync("ManagerDoctor")).Returns(Task.FromResult(true));
            mockIdentityServiceManager.Setup(x => x.IsInRoleAsync("NormalDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceManager.Setup(x => x.IsInRoleAsync("User")).Returns(Task.FromResult(false));
            mockIdentityServiceManager.Setup(x => x.GetCurrentUserId()).Returns(UserIdManager);

            identityServiceManager = mockIdentityServiceManager.Object;

            var mockIdentityServiceDoctor = new Mock<IIdentityService>();
            mockIdentityServiceDoctor.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "ManagerDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceDoctor.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "NormalDoctor")).Returns(Task.FromResult(true));
            mockIdentityServiceDoctor.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "User")).Returns(Task.FromResult(false));
            mockIdentityServiceDoctor.Setup(x => x.IsInRoleAsync("ManagerDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceDoctor.Setup(x => x.IsInRoleAsync("NormalDoctor")).Returns(Task.FromResult(true));
            mockIdentityServiceDoctor.Setup(x => x.IsInRoleAsync("User")).Returns(Task.FromResult(false));
            mockIdentityServiceDoctor.Setup(x => x.GetCurrentUserId()).Returns(UserIdDoctor);

            identityServiceDoctor = mockIdentityServiceDoctor.Object;

            var mockIdentityServiceUser = new Mock<IIdentityService>();
            mockIdentityServiceUser.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "ManagerDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceUser.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "NormalDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceUser.Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), "User")).Returns(Task.FromResult(true));
            mockIdentityServiceUser.Setup(x => x.IsInRoleAsync("ManagerDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceUser.Setup(x => x.IsInRoleAsync("NormalDoctor")).Returns(Task.FromResult(false));
            mockIdentityServiceUser.Setup(x => x.IsInRoleAsync("User")).Returns(Task.FromResult(true));
            mockIdentityServiceUser.Setup(x => x.GetCurrentUserId()).Returns(UserIdUser);

            identityServiceManager = mockIdentityServiceManager.Object;
            identityServiceUser = mockIdentityServiceUser.Object;

            veterinaryDbContext = new VeterinaryDbContext(options);
            veterinaryDbContext.Database.EnsureCreated();
            mockedRepositories = new UnitTestRepositories(veterinaryDbContext);

            var users = new List<VeterinaryUser>
            {
                new VeterinaryUser
                {
                    Id = UserIdManager,
                    UserName = "manager@veterinary.hu",
                    Email = "manager@veterinary.hu"
                },
                new VeterinaryUser
                {
                    Id = UserIdUser,
                    UserName = "user1@veterinary.hu",
                    Email = "user1@veterinary.hu"
                },
                new VeterinaryUser
                {
                    Id = UserIdDoctor,
                    UserName = "doctor@veterinary.hu",
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

            veterinaryDbContext.Users.AddRange(users);
            veterinaryDbContext.SaveChanges();
        }


        public void Dispose()
        {
            veterinaryDbContext.Dispose();
            connection.Dispose();
        }
    }
}
