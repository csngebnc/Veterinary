using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Veterinary.Application.Features.AnimalFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;
using FluentAssertions;
using static Veterinary.Application.Features.AnimalFeatures.Commands.UpdateAnimalCommand;

namespace Veterinary.Tests.UnitTests.AnimalTests
{
    public class AnimalTest : UnitTestBase
    {
        public AnimalTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateAnimal()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange();

            var command = new CreateAnimalCommand
            {
                UserId = identityServiceManager.GetCurrentUserId(),
                Data = new CreateAnimalCommand.CreateAnimalCommandData
                {
                    Name = "Madzag",
                    DateOfBirth = new DateTime(2018, 4, 23),
                    Sex = "hím",
                    SpeciesId = bunny.Id
                }
            };
            var commandHandler = new CreateAnimalCommandHandler(mockedRepositories.AnimalRepository, default);

            // Act 
            await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            var userAnimals = await mockedRepositories.AnimalRepository
                .GetAllAsQueryable()
                .Where(x => x.OwnerId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.NotEmpty(userAnimals);
            Assert.Single(userAnimals);
        }

        [Fact]
        public async Task Test_UpdateAnimal()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(bunny.Id);

            // Act
            var command = new UpdateAnimalCommand
            {
                AnimalId = animal.Id,
                Data = new UpdateAnimalCommandData
                {
                    Name = animal.Name,
                    DateOfBirth = animal.DateOfBirth,
                    Sex = animal.Sex,
                    SpeciesId = animal.SpeciesId,
                    SubSpecies = "lógófülü",
                    Weight = 1.5
                }
            };

            var handler = new UpdateAnimalCommandHandler(mockedRepositories.AnimalRepository, identityServiceManager);
            await handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.Equal("lógófülü", animal.SubSpecies);
            Assert.Equal(1.5, animal.Weight);
        }

        [Fact]
        public async Task Test_DeleteAnimal()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(bunny.Id);

            var command = new DeleteAnimalCommand
            {
                AnimalId = animal.Id
            };
            var commandHandler = new DeleteAnimalCommandHandler(mockedRepositories.AnimalRepository, identityServiceManager);

            // Act 
            await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            var userAnimals = await mockedRepositories.AnimalRepository
                .GetAllAsQueryable()
                .IgnoreQueryFilters()
                .Where(x => x.OwnerId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.True(animal.IsDeleted);
            Assert.Empty(userAnimals.Where(animal => !animal.IsDeleted).ToList());
        }

        [Fact]
        public async Task Test_DeleteAnimal_ThrowsForbiddenException()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(bunny.Id);

            var command = new DeleteAnimalCommand
            {
                AnimalId = animal.Id
            };
            var commandHandler = new DeleteAnimalCommandHandler(mockedRepositories.AnimalRepository, identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await commandHandler.Handle(command, CancellationToken.None);
            await Assert.ThrowsAsync<ForbiddenException>(action);
            Assert.False(animal.IsDeleted);
        }

        [Fact]
        public async Task Test_ArchiveAnimal()
        {
            // Arrange
            var bunny = await CreateAnimalSpecies_ForArrange();
            var animal = await CreateAnimal_ForArrange(bunny.Id);

            var command = new UpdateAnimalArchiveStatusCommand
            {
                AnimalId = animal.Id
            };
            var commandHandler = new UpdateAnimalArchiveStatusCommandHandler(mockedRepositories.AnimalRepository, identityServiceManager);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(animal.IsArchived);
        }

        private async Task<AnimalSpecies> CreateAnimalSpecies_ForArrange()
        {
            var bunny = new AnimalSpecies
            {
                Id = Guid.NewGuid(),
                Name = "nyúl"
            };
            await mockedRepositories.AnimalSpeciesRepository.InsertAsync(bunny);

            return bunny;
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
