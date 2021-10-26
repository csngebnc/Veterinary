using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Features.VaccineFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.Vaccination;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.ManagerTests
{
    public class VaccineTest : UnitTestBase
    {
        public VaccineTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateVaccine()
        {
            // Arrange
            var command = new CreateVaccineCommand
            {
                Name = "Oltás"
            };

            var handler = new CreateVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var vaccine = await mockedRepositories.VaccineRepository.GetAllAsQueryable().ToListAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(vaccine);
            Assert.Single(vaccine);
        }

        [Fact]
        public async Task Test_CreateVaccine_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateVaccineCommand
            {
                Name = "Oltás"
            };

            var handler = new CreateVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateVaccine()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();
            var command = new UpdateVaccineCommand
            {
                Id = vaccine.Id,
                Name = "Új oltás",
            };

            var handler = new UpdateVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.Equal("Új oltás", vaccine.Name);
        }

        [Fact]
        public async Task Test_UpdateVaccine_ThrowsForbiddenException()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();
            var command = new UpdateVaccineCommand
            {
                Id = vaccine.Id,
                Name = "Gyógyszer"
            };

            var handler = new UpdateVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DisableVaccine()
        {
            // Arrange
            var Vaccine = await CreateVaccine_ForArrange();
            var command = new UpdateVaccineStatusCommand
            {
                Id = Vaccine.Id
            };

            var handler = new UpdateVaccineStatusCommandHandler(mockedRepositories.VaccineRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.True(Vaccine.IsInactive);
        }

        [Fact]
        public async Task Test_EnableVaccine()
        {
            // Arrange
            var Vaccine = await CreateVaccine_ForArrange(true);
            var command = new UpdateVaccineStatusCommand
            {
                Id = Vaccine.Id
            };

            var handler = new UpdateVaccineStatusCommandHandler(mockedRepositories.VaccineRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.False(Vaccine.IsInactive);
        }

        [Fact]
        public async Task Test_UpdateVaccineStatus_ThrowsForbiddenException()
        {
            // Arrange
            var Vaccine = await CreateVaccine_ForArrange(true);
            var command = new UpdateVaccineStatusCommand
            {
                Id = Vaccine.Id
            };

            var handler = new UpdateVaccineStatusCommandHandler(mockedRepositories.VaccineRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteVaccine()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();

            var command = new DeleteVaccineCommand
            {
                VaccineId = vaccine.Id
            };

            var handler = new DeleteVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var vaccines = await mockedRepositories.VaccineRepository.GetAllAsQueryable().ToListAsync();
            Assert.Empty(vaccines);
        }

        [Fact]
        public async Task Test_DeleteVaccine_ThrowsForbiddenException()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();

            var command = new DeleteVaccineCommand
            {
                VaccineId = vaccine.Id
            };

            var handler = new DeleteVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteVaccine_ThrowsMethodNotAllowedException()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();
            var species = await CreateAnimalSpecies_ForArrange("nyúl");

            await CreateAnimalWithVaccineRecord_ForArrange(vaccine.Id, species.Id);

            var command = new DeleteVaccineCommand
            {
                VaccineId = vaccine.Id
            };

            var handler = new DeleteVaccineCommandHandler(mockedRepositories.VaccineRepository, identityServiceManager);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<MethodNotAllowedException>(action);
        }

        private async Task<AnimalSpecies> CreateAnimalSpecies_ForArrange(string speciesName, bool isInactive = false)
        {
            var species = new AnimalSpecies
            {
                Name = speciesName,
                IsInactive = isInactive
            };
            await mockedRepositories.AnimalSpeciesRepository.InsertAsync(species);

            return species;
        }

        private async Task<Vaccine> CreateVaccine_ForArrange(bool isInactive = false)
        {
            var vaccine = new Vaccine
            {
                Name = "Oltás",
                IsInactive = isInactive
            };
            await mockedRepositories.VaccineRepository.InsertAsync(vaccine);

            return vaccine;
        }

        private async Task CreateAnimalWithVaccineRecord_ForArrange(Guid vaccineId, Guid speciesId)
        {
            var animal = new Animal
            {
                Name = "Madzag",
                DateOfBirth = new DateTime(2018, 4, 23),
                Sex = "hím",
                SpeciesId = speciesId,
                OwnerId = identityServiceManager.GetCurrentUserId(),
                VaccinationRecords = new List<VaccineRecord>
                 {
                     new VaccineRecord
                     {
                         VaccineId = vaccineId,
                         Date = new DateTime()
                     }
                 }
            };

            await mockedRepositories.AnimalRepository.InsertAsync(animal);
        }
    }
}
