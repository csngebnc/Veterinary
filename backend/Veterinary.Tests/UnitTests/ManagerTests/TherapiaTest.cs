using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Features.TherapiaFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Domain.Entities.TherapiaEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.ManagerTests
{
    public class TherapiaTest : UnitTestBase
    {
        public TherapiaTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateTherapia()
        {
            // Arrange
            var command = new CreateTherapiaCommand
            {
                Data = new CreateTherapiaCommandData
                {
                    Name = "Szolgáltatás",
                    Price = 100
                }
            };

            var handler = new CreateTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var therapias = await mockedRepositories.TherapiaRepository.GetAllAsQueryable().ToListAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(therapias);
            Assert.Single(therapias);
        }

        [Fact]
        public async Task Test_CreateTherapia_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateTherapiaCommand
            {
                Data = new CreateTherapiaCommandData
                {
                    Name = "Szolgáltatás",
                    Price = 100
                }
            };

            var handler = new CreateTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateTherapia()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange();
            var command = new UpdateTherapiaCommand
            {
                Data = new UpdateTherapiaCommandData
                {
                    Id = therapia.Id,
                    Name = "Új szolgáltatás",
                    Price = 200
                }
            };

            var handler = new UpdateTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.Equal("Új szolgáltatás", therapia.Name);
            Assert.Equal(200, therapia.Price);
        }

        [Fact]
        public async Task Test_UpdateTherapia_ThrowsForbiddenException()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange();
            var command = new UpdateTherapiaCommand
            {
                Data = new UpdateTherapiaCommandData
                {
                    Id = therapia.Id,
                    Name = "Új szolgáltatás",
                    Price = 200,
                }
            };

            var handler = new UpdateTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DisableTherapia()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange();
            var command = new UpdateTherapiaStatusCommand
            {
                Id = therapia.Id
            };

            var handler = new UpdateTherapiaStatusCommandHandler(mockedRepositories.TherapiaRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.True(therapia.IsInactive);
        }

        [Fact]
        public async Task Test_EnableTherapia()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange(true);
            var command = new UpdateTherapiaStatusCommand
            {
                Id = therapia.Id
            };

            var handler = new UpdateTherapiaStatusCommandHandler(mockedRepositories.TherapiaRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.False(therapia.IsInactive);
        }

        [Fact]
        public async Task Test_UpdateTherapiaStatus_ThrowsForbiddenException()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange(true);
            var command = new UpdateTherapiaStatusCommand
            {
                Id = therapia.Id
            };

            var handler = new UpdateTherapiaStatusCommandHandler(mockedRepositories.TherapiaRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteTherapia()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange();

            var command = new DeleteTherapiaCommand
            {
                TherapiaId = therapia.Id
            };

            var handler = new DeleteTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var species = await mockedRepositories.TherapiaRepository.GetAllAsQueryable().ToListAsync();
            Assert.Empty(species);
        }

        [Fact]
        public async Task Test_DeleteTherapia_ThrowsForbiddenException()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange();

            var command = new DeleteTherapiaCommand
            {
                TherapiaId = therapia.Id
            };

            var handler = new DeleteTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteTherapia_ThrowsMethodNotAllowedException()
        {
            // Arrange
            var therapia = await CreateTherapia_ForArrange();

            await CreateMedicalRecordUsingTherapia_ForArrange(therapia.Id);
            var command = new DeleteTherapiaCommand
            {
                TherapiaId = therapia.Id
            };

            var handler = new DeleteTherapiaCommandHandler(mockedRepositories.TherapiaRepository, identityServiceManager);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<MethodNotAllowedException>(action);
        }


        private async Task<Therapia> CreateTherapia_ForArrange(bool isInactive = false)
        {
            var therapia = new Therapia
            {
                Name = "Szolgáltatás",
                Price = 100,
                IsInactive = isInactive
            };
            await mockedRepositories.TherapiaRepository.InsertAsync(therapia);

            return therapia;
        }

        private async Task CreateMedicalRecordUsingTherapia_ForArrange(Guid TherapiaId)
        {
            var medicalRecord = new MedicalRecord
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                OwnerEmail = "email@email.hu",
                TherapiaRecords = new List<TherapiaRecord>
                {
                    new TherapiaRecord
                    {
                        TherapiaId = TherapiaId,
                        Amount = 1
                    }
                }
            };

            await mockedRepositories.MedicalRecordRepository.InsertAsync(medicalRecord);
        }
    }
}
