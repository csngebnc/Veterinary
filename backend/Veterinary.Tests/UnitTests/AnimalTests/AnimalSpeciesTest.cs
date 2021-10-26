using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Features.AnimalSpeciesFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.AnimalTests
{
    public class AnimalSpeciesTest : UnitTestBase
    {
        public AnimalSpeciesTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateAnimalSpecies()
        {
            // Arrange
            var command = new CreateAnimalSpeciesCommand
            {
                Name = "nyúl"
            };

            var handler = new CreateAnimalSpeciesCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var species = await mockedRepositories.AnimalSpeciesRepository.GetAllAsQueryable().ToListAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(species);
            Assert.Single(species);
        }

        [Fact]
        public async Task Test_CreateAnimalSpecies_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateAnimalSpeciesCommand
            {
                Name = "nyúl"
            };

            var handler = new CreateAnimalSpeciesCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateAnimalSpecies()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl");
            var command = new UpdateAnimalSpeciesCommand
            {
                Id = bunny.Id,
                Name = "nyulacska"
            };

            var handler = new UpdateAnimalSpeciesCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var species = await mockedRepositories.AnimalSpeciesRepository.GetAllAsQueryable().FirstAsync();
            Assert.Equal("nyulacska", bunny.Name);
        }

        [Fact]
        public async Task Test_UpdateAnimalSpecies_ThrowsForbiddenException()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl");
            var command = new UpdateAnimalSpeciesCommand
            {
                Id = bunny.Id,
                Name = "nyulacska"
            };

            var handler = new UpdateAnimalSpeciesCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DisableAnimalSpecies()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl");
            var command = new UpdateAnimalSpeciesStatusCommand
            {
                Id = bunny.Id
            };

            var handler = new UpdateAnimalSpeciesStatusCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.True(bunny.IsInactive);
        }

        [Fact]
        public async Task Test_EnableAnimalSpecies()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl", true);
            var command = new UpdateAnimalSpeciesStatusCommand
            {
                Id = bunny.Id
            };

            var handler = new UpdateAnimalSpeciesStatusCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.False(bunny.IsInactive);
        }

        [Fact]
        public async Task Test_UpdateAnimalSpeciesStatus_ThrowsForbiddenException()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl", true);
            var command = new UpdateAnimalSpeciesStatusCommand
            {
                Id = bunny.Id
            };

            var handler = new UpdateAnimalSpeciesStatusCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteAnimalSpecies()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl", true);
            var command = new DeleteAnimalSpeciesCommand
            {
                SpeciesId = bunny.Id
            };

            var handler = new DeleteAnimalSpeciesCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var species = await mockedRepositories.AnimalSpeciesRepository.GetAllAsQueryable().ToListAsync();
            Assert.Empty(species);
        }

        [Fact]
        public async Task Test_DeleteAnimalSpecies_ThrowsForbiddenException()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange("nyúl", true);
            var animal = await CreateAnimal_ForArrange(bunny.Id);
            var command = new DeleteAnimalSpeciesCommand
            {
                SpeciesId = bunny.Id
            };

            var handler = new DeleteAnimalSpeciesCommandHandler(mockedRepositories.AnimalSpeciesRepository, identityServiceManager);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<MethodNotAllowedException>(action);
        }


        private async Task<AnimalSpecies> CreateAnimalSpecies_ForArrange(string speciesName, bool isInactive = false)
        {
            var species = new AnimalSpecies
            {
                Id = Guid.NewGuid(),
                Name = speciesName,
                IsInactive = isInactive
            };
            await mockedRepositories.AnimalSpeciesRepository.InsertAsync(species);

            return species;
        }

        private async Task<Animal> CreateAnimal_ForArrange(Guid speciesId)
        {
            var animal = new Animal
            {
                OwnerId = identityServiceManager.GetCurrentUserId(),
                Name = "Madzag",
                DateOfBirth = new DateTime(2018, 4, 23),
                Sex = "hím",
                SpeciesId = speciesId
            };
            await mockedRepositories.AnimalRepository.InsertAsync(animal);

            return animal;
        }
    }

}
