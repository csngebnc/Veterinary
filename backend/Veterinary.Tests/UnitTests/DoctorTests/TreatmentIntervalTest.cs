using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Veterinary.Application.Features.Doctor.TreatmentIntervalFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.TreatmentEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.DoctorTests
{
    public class TreatmentIntervalTest : UnitTestBase
    {
        public TreatmentIntervalTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateTreatmentInterval()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var command = new CreateTreatmentIntervalCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Data = new CreateTreatmentIntervalCommandData
                {
                    TreatmentId = treatment.Id,
                    DayOfWeek = 1,
                    StartHour = 10,
                    StartMin = 0,
                    EndHour = 11,
                    EndMin = 0
                }
            };

            var handler = new CreateTreatmentIntervalCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository, 
                identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var treatmentsForUser = await mockedRepositories.TreatmentIntervalRepository
                .GetAllAsQueryable()
                .Where(interval => interval.TreatmentId == treatment.Id)
                .ToListAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(treatmentsForUser);
            Assert.Single(treatmentsForUser);
        }

        [Fact]
        public async Task Test_CreateTreatmentInterval_ThrowsForbiddenException()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var command = new CreateTreatmentIntervalCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Data = new CreateTreatmentIntervalCommandData
                {
                    TreatmentId = treatment.Id,
                    DayOfWeek = 1,
                    StartHour = 10,
                    StartMin = 0,
                    EndHour = 11,
                    EndMin = 0
                }
            };

            var handler = new CreateTreatmentIntervalCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_UpdateTreatmentInterval()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);
            var command = new UpdateTreatmentIntervalCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Data = new UpdateTreatmentIntervalCommandData
                {
                    Id = interval.Id,
                    DayOfWeek = 1,
                    StartHour = 11,
                    StartMin = 0,
                    EndHour = 12,
                    EndMin = 0
                }
            };

            var handler = new UpdateTreatmentIntervalCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository, 
                identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var intervals = await mockedRepositories.TreatmentIntervalRepository
                .GetAllAsQueryable().Where(interval => interval.TreatmentId == treatment.Id)
                .ToListAsync();

            Assert.NotEmpty(intervals);
            Assert.Single(intervals);

        }

        [Fact]
        public async Task Test_UpdateTreatmentInterval_ThrowsForbiddenException()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);
            var command = new UpdateTreatmentIntervalCommand
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                Data = new UpdateTreatmentIntervalCommandData
                {
                    Id = interval.Id,
                    DayOfWeek = 1,
                    StartHour = 11,
                    StartMin = 0,
                    EndHour = 12,
                    EndMin = 0
                }
            };

            var handler = new UpdateTreatmentIntervalCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DisableTreatmentInterval()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);
            var command = new UpdateTreatmentIntervalStatusCommand
            {
                TreatmentIntervalId = interval.Id,
            };

            var handler = new UpdateTreatmentIntervalStatusCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository, 
                identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.True(interval.IsInactive);

        }

        [Fact]
        public async Task Test_EnableTreatmentInterval()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange();
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id, true);
            var command = new UpdateTreatmentIntervalStatusCommand
            {
                TreatmentIntervalId = interval.Id,
            };

            var handler = new UpdateTreatmentIntervalStatusCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            Assert.False(interval.IsInactive);

        }

        [Fact]
        public async Task Test_UpdateTreatmentIntervalStatus_ThrowsForbiddenException()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);
            var command = new UpdateTreatmentIntervalStatusCommand
            {
                TreatmentIntervalId = interval.Id,
            };

            var handler = new UpdateTreatmentIntervalStatusCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteTreatmentInterval()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var command = new DeleteTreatmentIntervalCommand
            {
                TreatmentIntervalId = interval.Id,
            };

            var handler = new DeleteTreatmentIntervalCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository, 
                identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var intervals = await mockedRepositories.TreatmentIntervalRepository
                .GetAllAsQueryable().Where(interval => interval.TreatmentId == treatment.Id)
                .ToListAsync();

            Assert.Empty(intervals);

        }

        [Fact]
        public async Task Test_DeleteTreatmentInterval_ThrowsForbiddenException()
        {
            // Arrange
            var treatment = await CreateTreatment_ForArrange(true);
            var interval = await CreateTreatmentInterval_ForArrange(treatment.Id);

            var command = new DeleteTreatmentIntervalCommand
            {
                TreatmentIntervalId = interval.Id,
            };

            var handler = new DeleteTreatmentIntervalCommandHandler(
                mockedRepositories.TreatmentRepository,
                mockedRepositories.TreatmentIntervalRepository,
                identityServiceUser);

            // Act & Assert
            Func<Task> action = async () => await handler.Handle(command, default);

            await Assert.ThrowsAsync<ForbiddenException>(action);
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

        private async Task<TreatmentInterval> CreateTreatmentInterval_ForArrange(Guid treatmentId, bool isInactive = false)
        {
            var interval = new TreatmentInterval
            {
                TreatmentId = treatmentId,
                DayOfWeek = 1,
                StartHour = 10,
                StartMin = 0,
                EndHour = 11,
                EndMin = 0,
                IsInactive = isInactive
            };

            await mockedRepositories.TreatmentIntervalRepository.InsertAsync(interval);

            return interval;
        }
    }
}
