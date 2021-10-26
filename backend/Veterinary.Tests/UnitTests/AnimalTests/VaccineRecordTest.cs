using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Features.VaccineRecordFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.Vaccination;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.AnimalTests
{
    public class VaccineRecordRecordTest : UnitTestBase
    {
        public VaccineRecordRecordTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateVaccineRecord()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();
            var species = await CreateAnimalSpecies_ForArrange("nyúl");
            var animal = await CreateAnimal_ForArrange(species.Id);

            var command = new CreateVaccineRecordCommand
            {
                Data = new CreateVaccineRecordCommandData
                {
                    AnimalId = animal.Id,
                    Date = new DateTime(),
                    VaccineId = vaccine.Id
                }
            };

            var handler = new CreateVaccineRecordCommandHandler(
                mockedRepositories.AnimalRepository,
                mockedRepositories.VaccineRepository,
                mockedRepositories.VaccineRecordRepository,
                identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var vaccineRecord = await mockedRepositories.VaccineRecordRepository
                .GetAllAsQueryable()
                .Where(record => record.AnimalId == animal.Id)
                .ToListAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(vaccineRecord);
            Assert.Single(vaccineRecord);
        }

        [Fact]
        public async Task Test_CreateVaccineRecord_ThrowsForbiddenException()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();
            var species = await CreateAnimalSpecies_ForArrange("nyúl");
            var animal = await CreateAnimal_ForArrange(species.Id);

            var command = new CreateVaccineRecordCommand
            {
                Data = new CreateVaccineRecordCommandData
                {
                    AnimalId = animal.Id,
                    Date = new DateTime(),
                    VaccineId = vaccine.Id
                }
            };

            var handler = new CreateVaccineRecordCommandHandler(
                mockedRepositories.AnimalRepository,
                mockedRepositories.VaccineRepository,
                mockedRepositories.VaccineRecordRepository,
                identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateVaccineRecord()
        {
            // Arrange
            var vaccineRecord = await CreateVaccineRecord_ForArrange();
            var newDate = new DateTime().AddDays(1);
            var command = new UpdateVaccineRecordCommand
            {
                Data = new UpdateVaccineRecordCommandData
                {
                    Id = vaccineRecord.Id,
                    Date = newDate,
                    AnimalId = vaccineRecord.AnimalId,
                    VaccineId = vaccineRecord.VaccineId
                }
            };

            var handler = new UpdateVaccineRecordCommandHandler(
                mockedRepositories.VaccineRecordRepository,
                mockedRepositories.AnimalRepository,
                identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.Equal(newDate, vaccineRecord.Date);
        }

        [Fact]
        public async Task Test_UpdateVaccineRecord_ThrowsForbiddenException()
        {
            // Arrange
            var vaccineRecord = await CreateVaccineRecord_ForArrange();
            var newDate = new DateTime().AddDays(1);
            var command = new UpdateVaccineRecordCommand
            {
                Data = new UpdateVaccineRecordCommandData
                {
                    Id = vaccineRecord.Id,
                    Date = newDate,
                    AnimalId = vaccineRecord.AnimalId,
                    VaccineId = vaccineRecord.VaccineId
                }
            };

            var handler = new UpdateVaccineRecordCommandHandler(
                mockedRepositories.VaccineRecordRepository,
                mockedRepositories.AnimalRepository,
                identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteVaccineRecord()
        {
            // Arrange
            var vaccineRecord = await CreateVaccineRecord_ForArrange();

            var command = new DeleteVaccineRecordCommand
            {
                RecordId = vaccineRecord.Id
            };

            var handler = new DeleteVaccineRecordCommandHandler(
                mockedRepositories.VaccineRecordRepository,
                mockedRepositories.AnimalRepository,
                identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var VaccineRecords = await mockedRepositories.VaccineRecordRepository.GetAllAsQueryable().ToListAsync();
            Assert.Empty(VaccineRecords);
        }

        [Fact]
        public async Task Test_DeleteVaccineRecord_ThrowsForbiddenException()
        {
            // Arrange
            var vaccineRecord = await CreateVaccineRecord_ForArrange();

            var command = new DeleteVaccineRecordCommand
            {
                RecordId = vaccineRecord.Id
            };

            var handler = new DeleteVaccineRecordCommandHandler(
                mockedRepositories.VaccineRecordRepository,
                mockedRepositories.AnimalRepository,
                identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
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

        private async Task<Vaccine> CreateVaccine_ForArrange()
        {
            var vaccine = new Vaccine { Name = "Oltás" };
            await mockedRepositories.VaccineRepository.InsertAsync(vaccine);

            return vaccine;
        }

        private async Task<Animal> CreateAnimal_ForArrange(Guid speciesId)
        {
            var animal = new Animal
            {
                Name = "Madzag",
                DateOfBirth = new DateTime(2018, 4, 23),
                Sex = "hím",
                SpeciesId = speciesId,
                OwnerId = identityServiceManager.GetCurrentUserId()
            };

            await mockedRepositories.AnimalRepository.InsertAsync(animal);

            return animal;
        }

        private async Task<VaccineRecord> CreateVaccineRecord_ForArrange()
        {
            // Arrange
            var vaccine = await CreateVaccine_ForArrange();
            var species = await CreateAnimalSpecies_ForArrange("nyúl");
            var animal = await CreateAnimal_ForArrange(species.Id);

            var record = new VaccineRecord
            {
                AnimalId = animal.Id,
                Date = new DateTime(),
                VaccineId = vaccine.Id
            };

            await mockedRepositories.VaccineRecordRepository.InsertAsync(record);
            return record;
        }
    }
}
