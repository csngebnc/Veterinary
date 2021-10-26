using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Veterinary.Application.Features.Doctor.HolidayFeatures.Commands;
using Veterinary.Application.Validation.ProblemDetails.Exceptions;
using Veterinary.Domain.Entities.Doctor.HolidayEntities;
using Veterinary.Tests.UnitTests.Basics;
using Xunit;

namespace Veterinary.Tests.UnitTests.DoctorTests
{
    public class HolidayTest : UnitTestBase
    {
        public HolidayTest() : base()
        {
        }

        [Fact]
        public async Task Test_CreateHoliday()
        {
            // Arrange
            var command = new CreateHolidayCommand
            {
                Data = new CreateHolidayCommandData
                {
                    DoctorId = identityServiceManager.GetCurrentUserId(),
                    StartDate = new DateTime(),
                    EndDate = new DateTime().AddDays(1)
                }
            };

            var handler = new CreateHolidayCommandHandler(mockedRepositories.HolidayRepository, identityServiceManager);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            var holidays = await mockedRepositories.HolidayRepository
                .GetAllAsQueryable()
                .Where(holiday => holiday.DoctorId == identityServiceManager.GetCurrentUserId())
                .ToListAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(holidays);
            Assert.Single(holidays);
        }
        
        [Fact]
        public async Task Test_CreateHoliday_ThrowsForbiddenException()
        {
            // Arrange
            var command = new CreateHolidayCommand
            {
                Data = new CreateHolidayCommandData
                {
                    DoctorId = identityServiceManager.GetCurrentUserId(),
                    StartDate = new DateTime(),
                    EndDate = new DateTime().AddDays(1)
                }
            };

            var handler = new CreateHolidayCommandHandler(mockedRepositories.HolidayRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }
        
        [Fact]
        public async Task Test_UpdateHoliday()
        {
            // Arrange
            var holiday = await CreateHoliday_ForArrange();

            var startDate = new DateTime().AddDays(1);
            var endDate = new DateTime().AddDays(2);

            var command = new UpdateHolidayCommand
            {
                Data = new UpdateHolidayCommandData
                {
                    HolidayId = holiday.Id,
                    StartDate = startDate,
                    EndDate = endDate
                }
            };

            var handler = new UpdateHolidayCommandHandler(mockedRepositories.HolidayRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var created_holiday = await mockedRepositories.HolidayRepository.GetAllAsQueryable().FirstAsync();
            Assert.Equal(startDate, created_holiday.StartDate);
            Assert.Equal(endDate, created_holiday.EndDate);
        }
        
        [Fact]
        public async Task Test_UpdateHoliday_ThrowsForbiddenException()
        {
            // Arrange
            var holiday = await CreateHoliday_ForArrange();

            var startDate = new DateTime().AddDays(1);
            var endDate = new DateTime().AddDays(2);

            var command = new UpdateHolidayCommand
            {
                Data = new UpdateHolidayCommandData
                {
                    HolidayId = holiday.Id,
                    StartDate = startDate,
                    EndDate = endDate
                }
            };

            var handler = new UpdateHolidayCommandHandler(mockedRepositories.HolidayRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        [Fact]
        public async Task Test_DeleteHoliday()
        {
            // Arrange
            var holiday = await CreateHoliday_ForArrange();
            var command = new DeleteHolidayCommand
            {
                HolidayId = holiday.Id
            };

            var handler = new DeleteHolidayCommandHandler(mockedRepositories.HolidayRepository, identityServiceManager);

            // Act
            await handler.Handle(command, default);

            // Assert
            var holidays = await mockedRepositories.HolidayRepository.GetAllAsQueryable().ToListAsync();
            Assert.Empty(holidays);
        }

        [Fact]
        public async Task Test_DeleteHoliday_ThrowsForbiddenException()
        {
            // Arrange
            var holiday = await CreateHoliday_ForArrange();
            var command = new DeleteHolidayCommand
            {
                HolidayId = holiday.Id
            };

            var handler = new DeleteHolidayCommandHandler(mockedRepositories.HolidayRepository, identityServiceUser);

            // Act
            Func<Task> action = async () => await handler.Handle(command, default);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(action);
        }

        private async Task<Holiday> CreateHoliday_ForArrange()
        {
            var holiday = new Holiday
            {
                DoctorId = identityServiceManager.GetCurrentUserId(),
                StartDate = new DateTime(),
                EndDate = new DateTime().AddDays(1)
            };
            await mockedRepositories.HolidayRepository.InsertAsync(holiday);

            return holiday;
        }
    }
}
