using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Veterinary.Application.Features.MedicationFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.MedicalRecordEntities;
using Veterinary.Domain.Entities.MedicationEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.ManagerTests
{
    public class MedicationTest : UnitTestBase
    {
        public MedicationTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateMedication()
        {
            // Arrange
            var command = new CreateMedicationCommand
            {
                Data = new CreateMedicationCommandData
                {
                    Name = "Gyógyszer",
                    PricePerUnit = 100,
                    Unit = 1,
                    UnitName = "egység neve"
                }
            };

            var handler = new CreateMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var medications = await mockedRepositories.MedicationRepository.GetAllAsQueryable().ToListAsync();
            Assert.NotNull(result);
            Assert.NotEmpty(medications);
            Assert.Single(medications);
        }

        [Fact]
        public async Task Test_CreateMedication_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateMedicationCommand
            {
                Data = new CreateMedicationCommandData
                {
                    Name = "Gyógyszer",
                    PricePerUnit = 100,
                    Unit = 1,
                    UnitName = "egység neve"
                }
            };

            var handler = new CreateMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateMedication()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange();
            var command = new UpdateMedicationCommand
            {
                Data = new UpdateMedicationCommandData
                {
                    Id = medication.Id,
                    Name = "Új gyógyszer",
                    PricePerUnit = 1000,
                    Unit = 1,
                    UnitName = "egység neve"
                }
            };

            var handler = new UpdateMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.Equal("Új gyógyszer", medication.Name);
        }

        [Fact]
        public async Task Test_UpdateMedication_ThrowsForbiddenException()
        {
            // Arrange
            var bunny = await CreateMedication_ForArrange();
            var command = new UpdateMedicationCommand
            {
                Data = new UpdateMedicationCommandData
                {
                    Id = bunny.Id,
                    Name = "Gyógyszer",
                    PricePerUnit = 1000,
                    Unit = 1,
                    UnitName = "egység neve"
                }
            };

            var handler = new UpdateMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DisableMedication()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange();
            var command = new UpdateMedicationStatusCommand
            {
                Id = medication.Id
            };

            var handler = new UpdateMedicationStatusCommandHandler(mockedRepositories.MedicationRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.True(medication.IsInactive);
        }

        [Fact]
        public async Task Test_EnableMedication()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange(true);
            var command = new UpdateMedicationStatusCommand
            {
                Id = medication.Id
            };

            var handler = new UpdateMedicationStatusCommandHandler(mockedRepositories.MedicationRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.False(medication.IsInactive);
        }

        [Fact]
        public async Task Test_UpdateMedicationStatus_ThrowsForbiddenException()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange(true);
            var command = new UpdateMedicationStatusCommand
            {
                Id = medication.Id
            };

            var handler = new UpdateMedicationStatusCommandHandler(mockedRepositories.MedicationRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteMedication()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange();

            var command = new DeleteMedicationCommand
            {
                MedicationId = medication.Id
            };

            var handler = new DeleteMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var medications = await mockedRepositories.MedicationRepository.GetAllAsQueryable().ToListAsync();
            Assert.Empty(medications);
        }

        [Fact]
        public async Task Test_DeleteMedication_ThrowsForbiddenException()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange();

            var command = new DeleteMedicationCommand
            {
                MedicationId = medication.Id
            };

            var handler = new DeleteMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteMedication_ThrowsMethodNotAllowedException()
        {
            // Arrange
            var medication = await CreateMedication_ForArrange();

            await CreateMedicalRecordUsingMedication_ForArrange(medication.Id);
            var command = new DeleteMedicationCommand
            {
                MedicationId = medication.Id
            };

            var handler = new DeleteMedicationCommandHandler(mockedRepositories.MedicationRepository, identityServiceManager);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<MethodNotAllowedException>(action);
        }


        private async Task<Medication> CreateMedication_ForArrange(bool isInactive = false)
        {
            var medication = new Medication
            {
                Name = "Gyógyszer",
                PricePerUnit = 100,
                Unit = 1,
                UnitName = "egység neve",
                IsInactive = isInactive
            };
            await mockedRepositories.MedicationRepository.InsertAsync(medication);

            return medication;
        }

        private async Task CreateMedicalRecordUsingMedication_ForArrange(Guid medicationId)
        {
            var medicalRecord = new MedicalRecord
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                OwnerEmail = "email@email.hu",
                MedicationRecords = new List<MedicationRecord>
                {
                    new MedicationRecord
                    {
                        MedicationId = medicationId,
                        Amount = 1
                    }
                }
            };

            await mockedRepositories.MedicalRecordRepository.InsertAsync(medicalRecord);
        }
    }
}
