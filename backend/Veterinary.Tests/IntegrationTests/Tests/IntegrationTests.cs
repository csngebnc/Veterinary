using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Features.AnimalFeatures.Commands;
using Veterinary.Application.Features.AnimalSpeciesFeatures.Commands;
using Veterinary.Application.Features.AnimalSpeciesFeatures.Queries;
using Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands;
using Veterinary.Application.Features.Manager.Commands;
using Veterinary.Application.Features.VaccineRecordFeatures.Commands;
using Veterinary.Application.Shared.Dtos;
using Veterinary.Domain.Entities.AnimalEntities;
using Veterinary.Domain.Entities.AnimalRepository;
using Veterinary.Domain.Entities.AnimalSpeciesRepository;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Domain.Entities.Vaccination;
using Veterinary.Tests.IntegrationTests.Envinroment.Extensions;
using Xunit;

namespace Veterinary.Tests.IntegrationTests.Tests
{
    public class IntegrationTests : IntegrationTestBase
    {
        public IntegrationTests() : base()
        {
        }

        [Fact]
        public async Task CreateSpecies()
        {
            await factory.SeedInitOptions();
            var spec = new CreateAnimalSpeciesCommand { Name = "nyúl" };
            var response = await client.PostJsonAsync("api/species", spec, "manager");

            await response.AssertSuccessAsync();

            await factory.RunWithInjectionAsync(async (IAnimalSpeciesRepository speciesRepository) =>
            {
                var speciesList = await speciesRepository.GetAllAsQueryable().ToListAsync();
                var species = speciesList.First();

                Assert.NotEmpty(speciesList);
                Assert.Single(speciesList);
                Assert.Equal("nyúl", species.Name);
            });
        }

        [Fact]
        public async Task CreateTreatment()
        {
            await factory.SeedInitOptions();

            var command = new CreateTreatmentCommand
            {
                Data = new CreateTreatmentCommandData
                {
                    Name = "Kezelés",
                    Duration = 10
                }
            };

            var response = await client.PostJsonAsync("api/treatments", command.Data, "manager");
            await response.AssertSuccessAsync();

            await factory.RunWithInjectionAsync(async (ITreatmentRepository treatmentRepository) =>
            {
                var treatments = await treatmentRepository.GetAllAsQueryable().ToListAsync();
                var treatment = treatments.First();

                Assert.NotEmpty(treatments);
                Assert.Single(treatments);
                Assert.Equal("Kezelés", treatment.Name);
                Assert.Equal(10, treatment.Duration);
            });
        }

        [Fact]
        public async Task CreateTreatment_NotWorksForUser()
        {
            await factory.SeedInitOptions();

            var command = new CreateTreatmentCommand
            {
                Data = new CreateTreatmentCommandData
                {
                    Name = "Kezelés",
                    Duration = 10
                }
            };

            var response = await client.PostJsonAsync("api/treatments", command.Data, "user");
            await response.AssertStatusCodeAsync(System.Net.HttpStatusCode.Forbidden);

            await factory.RunWithInjectionAsync(async (ITreatmentRepository treatmentRepository) =>
            {
                var treatments = await treatmentRepository.GetAllAsQueryable().ToListAsync();

                Assert.Empty(treatments);
            });
        }

        [Fact]
        public async Task CreateVaccineRecord()
        {
            await factory.SeedInitOptions();

            var vaccineId = Guid.NewGuid();
            var animalId = Guid.NewGuid();

            await factory.RunWithInjectionAsync(async (
                IAnimalSpeciesRepository animalSpeciesRepository,
                IAnimalRepository animalRepository,
                IVaccineRepository vaccineRepository
                ) =>
            {
                var vaccine = await vaccineRepository.InsertAsync(new Vaccine { Name = "Oltás" });
                var species = await animalSpeciesRepository.InsertAsync(new AnimalSpecies { Name = "nyúl" });
                var animal = await animalRepository.InsertAsync(new Animal
                {
                    Name = "Madzag",
                    DateOfBirth = new DateTime(2018, 4, 23),
                    SpeciesId = species.Id,
                    OwnerId = factory.UserIdManager,
                    Sex = "hím"
                });

                vaccineId = vaccine.Id;
                animalId = animal.Id;
            });

            var command = new CreateVaccineRecordCommand
            {
                Data = new CreateVaccineRecordCommandData
                {
                    AnimalId = animalId,
                    VaccineId = vaccineId,
                    Date = DateTime.Today.AddDays(-1)
                }
            };

            var response = await client.PostJsonAsync("api/vaccines/record", command.Data, "manager");
            await response.AssertSuccessAsync();

            await factory.RunWithInjectionAsync(async (IVaccineRecordRepository vaccineRecordRepository) =>
            {
                var records = await vaccineRecordRepository.GetAllAsQueryable().ToListAsync();

                Assert.NotEmpty(records);
                Assert.Single(records);

                var record = records.First();
                Assert.Equal(DateTime.Today.AddDays(-1), record.Date);
                Assert.Equal(animalId, record.AnimalId);
                Assert.Equal(vaccineId, record.VaccineId);
            });
        }

        [Fact]
        public async Task SpeciesQuery()
        {
            await factory.SeedInitOptions();

            await factory.RunWithInjectionAsync(async (IAnimalSpeciesRepository animalSpeciesRepository) =>
            {
                await animalSpeciesRepository.InsertAsync(new AnimalSpecies { Name = "nyúl" });
                await animalSpeciesRepository.InsertAsync(new AnimalSpecies { Name = "macska" });
                await animalSpeciesRepository.InsertAsync(new AnimalSpecies { Name = "kutya" });
            });

            var response = await client.GetJsonAsync<List<AnimalSpeciesDto>>("api/species", "manager");

            Assert.NotEmpty(response);
            Assert.Equal(3, response.Count);
        }
    }
}
