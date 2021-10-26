using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Application.Features.Doctor.TreatmentFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.DoctorTests
{
    public class TreatmentTest : UnitTestBase
    {
        public TreatmentTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateTreatment()
        {
            // Arrange
            var command = new CreateTreatmentCommand
            {
                Data = new CreateTreatmentCommandData
                {
                    Name = "Kezelés",
                    Duration = 10
                }
            };

            var handler = new CreateTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var treatmentsForUser = await mockedRepositories.TreatmentRepository
                .GetAllAsQueryable()
                .Where(treatment => treatment.DoctorId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(treatmentsForUser);
            Assert.Single(treatmentsForUser);
        }

        [Fact]
        public async Task Test_CreateTreatment_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateTreatmentCommand
            {
                Data = new CreateTreatmentCommandData
                {
                    Name = "Kezelés",
                    Duration = 10
                }
            };

            var handler = new CreateTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateTreatment()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var command = new UpdateTreatmentCommand
            {
                Data = new UpdateTreatmentCommandData
                {
                    TreatmentId = treatment.Id,
                    Name = "Új kezelés",
                    Duration = 10
                }
            };

            var handler = new UpdateTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var treatmentsForUser = await mockedRepositories.TreatmentRepository
                .GetAllAsQueryable().Where(treatment => treatment.DoctorId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();
            
            Assert.Equal("Új kezelés", treatment.Name);
            Assert.Single(treatmentsForUser);

        }

        [Fact]
        public async Task Test_UpdateTreatment_ThrowsForbiddenException()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var command = new UpdateTreatmentCommand
            {
                Data = new UpdateTreatmentCommandData
                {
                    TreatmentId = treatment.Id,
                    Name = "Új kezelés",
                    Duration = 10
                }
            };

            var handler = new UpdateTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DisableTreatment()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var command = new UpdateTreatmentStatusCommand
            {
                TreatmentId = treatment.Id,
            };

            var handler = new UpdateTreatmentStatusCommandHandler(mockedRepositories.TreatmentRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.True(treatment.IsInactive);

        }

        [Fact]
        public async Task Test_EnableTreatment()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var command = new UpdateTreatmentStatusCommand
            {
                TreatmentId = treatment.Id,
            };

            var handler = new UpdateTreatmentStatusCommandHandler(mockedRepositories.TreatmentRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.False(treatment.IsInactive);

        }

        [Fact]
        public async Task Test_UpdateTreatmentStatus_ThrowsForbiddenException()
        {
            // Arrange
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var command = new UpdateTreatmentStatusCommand
            {
                TreatmentId = treatment.Id,
            };

            var handler = new UpdateTreatmentStatusCommandHandler(mockedRepositories.TreatmentRepository, identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteTreatment()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var command = new DeleteTreatmentCommand
            {
                TreatmentId = treatment.Id,
            };

            var handler = new DeleteTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var treatmentsForUser = await mockedRepositories.TreatmentRepository
                .GetAllAsQueryable().Where(treatment => treatment.DoctorId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.Empty(treatmentsForUser);

        }

        [Fact]
        public async Task Test_DeleteTreatment_ThrowsForbiddenException()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var command = new DeleteTreatmentCommand
            {
                TreatmentId = treatment.Id,
            };

            var handler = new DeleteTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteTreatment_ThrowsMethodNotAllowed()
        {
            // Arrange
            var treatment = await CreateTreatmentWithInterval_ForArrange();
            var command = new DeleteTreatmentCommand
            {
                TreatmentId = treatment.Id,
            };

            var handler = new DeleteTreatmentCommandHandler(mockedRepositories.TreatmentRepository, identityServiceManager);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<MethodNotAllowedException>(action);
        }


        private async Task<Treatment> CreateTreatment_ForArrange(bool isInactive = false)
        {
            var treatment = new Treatment
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Name = "Kezelés",
                Duration = 10,
                IsInactive = isInactive
            };

            await mockedRepositories.TreatmentRepository.InsertAsync(treatment);

            return treatment;
        }

        private async Task<Treatment> CreateTreatmentWithInterval_ForArrange(bool isInactive = false)
        {
            var treatment = new Treatment
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Name = "Kezelés",
                Duration = 10,
                IsInactive = isInactive,
                TreatmentIntervals = new List<TreatmentInterval> {  new TreatmentInterval
                {
                    DayOfWeek = 1,
                    StartHour = 10,
                    StartMin = 0,
                    EndHour = 11,
                    EndMin = 0                    
                } }
            };

            await mockedRepositories.TreatmentRepository.InsertAsync(treatment);

            return treatment;
        }
    }
}
